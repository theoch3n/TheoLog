#### 📅 **Date**: 2025-04-18
#### 🔖 **Tags**: #PluginAssembly  #Pipeline #Dynamics365 #Dataverse #PowerPlatform #EventProcessing

---
# 事件執行 Pipeline (Event Execution Pipeline)

這份筆記整理自 Microsoft Learn 文件，說明 Dynamics 365 / Power Platform Dataverse 中，事件處理子系統如何基於**訊息管線執行模型 (Message Pipeline Execution Model)** 來執行外掛程式 (Plug-ins)。

## 核心概念：事件執行 Pipeline

-   當使用者操作或 SDK 呼叫觸發 Dataverse 中的事件時，系統會產生一個**訊息 (Message)**。
-   此訊息包含業務實體資訊和核心操作資訊。
-   訊息會被傳遞到**事件執行管線 (Event Execution Pipeline)** 中，依序由平台核心操作和已註冊的外掛程式處理。
-   外掛程式可以讀取或修改流經管線的訊息內容。

---

## 架構與處理模式

事件執行管線可以處理**同步 (Synchronous)** 和**非同步 (Asynchronous)** 的事件：

* **同步執行 (Synchronous Execution):**
    * 註冊為同步執行的外掛程式和平台核心操作會**立即**在主執行緒中被觸發。
    * 同一階段內若有多個同步外掛程式，會依照註冊的**執行順序 (Execution Order/Rank)** 依序執行。
* **非同步執行 (Asynchronous Execution):**
    * 註冊為非同步執行的外掛程式請求會先被放入**佇列 (Queue)**。
    * 由**非同步服務 (Asynchronous Service)** 在背景稍後執行。
    * 適用於不需即時完成、耗時或不應阻礙 UI 的操作。

> **重要時間限制：**
> 無論同步或非同步，在沙箱 (Sandbox) 環境中執行的外掛程式都有 **2 分鐘** 的時間限制。超時會拋出 `System.TimeoutException`。建議將耗時操作改用非同步模式或其他背景處理。

---

## 管線階段 (Pipeline Stages)

事件執行管線包含多個階段，其中 4 個主要階段可供註冊自訂外掛程式：

| 事件類型       | 階段名稱                    | 階段編號 (`Stage`) | 描述                                            | 交易性     |
| :--------- | :---------------------- | :------------- | :-------------------------------------------- | :------ |
| Pre-Event  | **Pre-validation**      | **10**         | 主要操作**之前**執行。可能在資料庫交易**外部**。安全性檢查**之前**執行。    | 可能不在交易內 |
| Pre-Event  | **Pre-operation**       | **20**         | 主要操作**之前**執行。在資料庫交易**內部**執行。                  | **是**   |
|            | Platform Core Operation | 30             | 平台核心操作 (Create/Update/Delete)。**無法**註冊自訂外掛程式。 | 是       |
| Post-Event | **Post-operation**      | **40**         | 主要操作**之後**執行。在資料庫交易**內部**執行。                  | **是**   |

* **Pre-Event Stages (事件前階段):** 包含 `Pre-validation` 和 `Pre-operation`。
* **Post-Event Stage (事件後階段):** 主要指 `Post-operation`。

---

## 訊息處理 (Message Processing)

-   Web 服務呼叫的參數會被打包成 `OrganizationRequest` 訊息。
-   訊息依序傳遞給管線中的外掛程式。
-   外掛程式透過傳入 `Execute` 方法的 `IPluginExecutionContext` 來存取訊息上下文，可讀取或修改內容。
-   核心操作完成後，訊息變為 `OrganizationResponse`。
-   回應再傳遞給 Post-event 階段的外掛程式處理。
-   最終回應返回給原始呼叫者。

---

## 外掛程式註冊 (Plug-in Registration)

-   可將外掛程式註冊在核心平台操作**之前** (`Pre-event`) 或**之後** (`Post-event`)。
-   執行管線是針對**每個組織 (Organization)** 的，外掛程式需在目標組織中註冊才能運作。

---

## 資料庫交易 (Database Transactions)

-   外掛程式是否在資料庫交易內執行，取決於註冊的階段。
    * `Stage 20` (**Pre-operation**) 和 `Stage 40` (**Post-operation**) **保證**在交易內。
    * `Stage 10` (**Pre-validation**) 可能在交易外。
-   可透過 `IPluginExecutionContext` 的 `IsInTransaction` 屬性檢查。
-   **重要：** 如果在交易內執行的外掛程式 (`Stage 20` 或 `Stage 40`) 拋出未處理的例外，**整個交易將會回復 (Rollback)**。
-   交易回復會取消核心操作、尚未執行的同步外掛程式及相關工作流程。

---

## 總結

Dynamics 365/Dataverse 的事件執行管線提供了一個結構化的方式來處理業務操作和執行自訂程式碼。理解不同階段的特性（特別是執行時間點和交易性）、同步與非同步模式，對於開發可靠、高效能的外掛程式至關重要。