---
📅 **Date**: 2025-04-07
🔖 **Tags**: #WebResource #PowerApps #Dynamics365 #RibbonButton 
---
#### 📅 **Date**: 2025-04-07
#### 🔖 **Tags**: #WebResource #PowerApps #Dynamics365 #RibbonButton 

---
# Web Resource 與 Ribbon Workbench 筆記

## Web Resource (網頁資源)

-   Web Resource 是 Power Platform / Dynamics 365 中的一種元件類型。
-   它允許開發者將用戶端檔案（例如 **JavaScript** 程式碼、**CSS** 樣式表、**HTML** 頁面、圖片等）上傳並儲存到系統中。
-   這些資源可以在表單腳本、功能區按鈕 (Ribbon Button)、自訂頁面 (HTML Web Resource) 等多個地方被引用和使用，以擴充系統的用戶端功能和介面。

## Ribbon Workbench (功能區工作台)

**Ribbon Workbench** 是一個常用的社群工具 (XrmToolBox 插件)，用於客製化 Power Platform / Dynamics 365 應用程式的功能區 (Ribbon)。

### 將 formContext 傳遞給 Ribbon Button 命令

-   透過 Ribbon Workbench 設定功能區按鈕 (Ribbon Button) 的命令 (Command) 時，可以直接將目前表單的上下文 (`formContext`) 作為參數傳遞給對應的 JavaScript 函數。

> **設定步驟 (在 Command 定義中):**

1.  選取或建立一個 `Command`。
2.  在 Command 的設定區域中，找到 `Parameters` (或類似的參數設定區塊)。
3.  點擊 **`Add Parameter`** (新增參數)。
4.  在新增的參數類型中，選擇 **`Crm Parameter`**。
5.  從 `Crm Parameter` 的下拉選單中，選擇 **`PrimaryControl`**。
    * `PrimaryControl` 在表單的 Ribbon Button 命令中，會自動解析為 `formContext` 物件。當 JavaScript 函數接收到這個參數時，即可直接使用 `formContext` 的所有方法與屬性。

**效果：** 如此設定後，繫結到此 Command 的 JavaScript 函數，其第一個參數就會接收到 `formContext`，無需額外撰寫程式碼去獲取它。

```javascript
// 範例 JavaScript 函數 (假設已在 Command 中設定傳遞 PrimaryControl)
function handleMyRibbonButtonClick(formContext) {
  // 在這裡可以直接使用 formContext
  var accountName = formContext.getAttribute("name").getValue();
  alert("Account Name: " + accountName);
  // ... 其他操作 ...
}