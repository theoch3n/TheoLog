**1. HTML 類 WebResource**

- **基本結構範例:**

```html
<!DOCTYPE html>
<html lang="en">
	<head>
		<meta charset="UTF-8" />
		<title>WebResourceName</title>
	</head>
	<body>
		<h1>這是一個 HTML WebResource</h1>
		<p>這裡可以放表單、按鈕或其他元素。</p>
	</body>
	<script>
		// *** 只會在這個 WebResource 中使用到的邏輯寫在這裡 ***

		// 例如：初始化頁面上的特定按鈕事件
		function initializePage() {
			const myButton = document.getElementById("mySpecialButton");
			if (myButton) {
				myButton.addEventListener("click", function () {
					alert("按鈕被點擊了！此邏輯僅用於此頁面。");
				});
			}
		}

		// 頁面載入時執行初始化
		window.onload = initializePage;
	</script>
</html>
```

- **核心概念:**
    - **關注點分離**: 如上例所示，通用的 JavaScript 邏輯應抽離成獨立的 Library (Script WebResource) 並在 `<head>` 中引用。特定於此 HTML 頁面的邏輯（如 `initializePage` 函數）則直接寫在頁面底部的 `<script>` 標籤內。

**2. Script 類 WebResource (稱為 Library)**

此類 WebResource 包含 JavaScript 程式碼，並進一步分為兩種用途：

- **A. TableLibrary (表單/資料表函式庫):**
    - **命名規則:** 以其對應的 **Table (資料表) 的邏輯名稱**命名，**排除 Publisher Prefix**，並以 `Library` 結尾。例如：`account` Table 的 Library 應為 `AccountLibrary`，`art_temptable` Table 的 Library 應為 `TempTableLibrary`。
    - **用途:** 存放**僅與單一特定 Table 相關**的邏輯。
    - **範例 (結構):** 下方的 `art.Account` 就是一個 Account TableLibrary 的例子。
- **B. FunctionalLibrary (功能性函式庫):**
    - **命名規則:** 以 `Library` 結尾，名稱描述其功能，例如 `CommonUtilsLibrary` 或 `SapIntegrationLibrary`。
    - **用途:** 存放**會被兩個或以上 Table 或元件重用**的通用邏輯。
- **通用規範與結構範例 (以 Account TableLibrary 為例):**

```javascript
// 1. art 套件宣告 (確保命名空間存在)
var art = art || {};

// 2. Library 物件定義 (這裡是 Account TableLibrary)
art.Account = {
    // --- Library 內部變數 ---
    loaded: false, // 標記 onLoad 是否已執行完成
    notificationId: "accountinfo_notification", // 用於表單通知的 ID

    // --- Library 主要事件處理函數 ---
    /**
	 * 表單 OnLoad 事件的進入點
     - @param {Xrm.Events.EventContext} executionContext 執行上下文
     */
    onLoad: function (executionContext) {
        if (art.Account.loaded) {
            return; // 防止重複執行
        }
        const formContext = executionContext.getFormContext(); // 取得表單上下文
        console.log("Account form loaded.");

        // 協調執行其他初始化函數 (使用 formContext)
        art.Account.setupEditable(formContext);
        art.Account.setupMandatory(formContext);
        art.Account.setupOnchange(formContext);
        art.Account.setupOptions(formContext);
        art.Account.setupFilters(formContext);
        // ... 其他 onLoad 需要執行的邏輯

        art.Account.loaded = true;
    },

    /**
     - 設定欄位可編輯狀態
     - @param {Xrm.FormContext} formContext 表單上下文
     */
    setupEditable: function (formContext) {
        // 範例：根據帳戶類型設定 'websiteurl' 欄位是否可編輯
        const accountCategoryCode = formContext.getAttribute("accountcategorycode").getValue();
        const websiteControl = formContext.getControl("websiteurl");
        if (websiteControl) {
            // 假設只有 'Preferred Customer' (值為 1) 可以編輯網站
            websiteControl.setDisabled(accountCategoryCode !== 1);
        }
        console.log("Editable fields configured.");
    },

    /**
     - 設定欄位必填狀態 (此處範例為空，需依需求實作)
     - @param {Xrm.FormContext} formContext 表單上下文
     */
    setupMandatory: function (formContext) {
        // 範例：若 revenue 大於 100萬，則 'sic' 欄位必填
        const revenue = formContext.getAttribute("revenue")?.getValue(); // 使用可選鍊 "?."
        const sicControl = formContext.getControl("sic");
        if (sicControl) {
            if (revenue && revenue > 1000000) {
                sicControl.getAttribute().setRequiredLevel("required");
            } else {
                sicControl.getAttribute().setRequiredLevel("none");
            }
        }
        console.log("Mandatory fields configured.");
    },

    /**
     - 註冊欄位 OnChange 事件 (此處範例為空，需依需求實作)
     - @param {Xrm.FormContext} formContext 表單上下文
     */
    setupOnchange: function (formContext) {
        // 範例：當 'address1_country' 變更時，觸發 Account_countryOnChange 函數
        const countryAttr = formContext.getAttribute("address1_country");
        if (countryAttr) {
            // 注意：傳入 executionContext 給註冊的函數
            countryAttr.addOnChange(Account_countryOnChange);
        }
            console.log("OnChange events registered.");
    },

    /**
     - 設定選項集欄位選項 (此處範例為空，需依需求實作)
     - @param {Xrm.FormContext} formContext 表單上下文
     */
    setupOptions: function (formContext) {
        // 範例：移除 'industrycode' 選項集中的某個選項
            console.log("Option sets configured.");
    },

    /**
     - 設定查詢欄位篩選條件 (此處範例為空，需依需求實作)
     - @param {Xrm.FormContext} formContext 表單上下文
     */
    setupFilters: function (formContext) {
        // 範例：設定 'primarycontactid' 只顯示與目前客戶相關的連絡人
        const primaryContactControl = formContext.getControl("primarycontactid");
        if (primaryContactControl) {
            // 實作 addPreSearch 和 addCustomFilter 的邏輯
        }
            console.log("Lookup filters configured.");
    },

    // --- Library 內部輔助函數 ---
    /**
     - 僅供 Library 內部使用的本地函數範例 1
     - @param {string} param1 參數1
     - @param {number} param2 參數2
     */
    localFunction1: function (param1, param2) {
        // ... 內部邏輯 ...
        console.log(`Local function 1 called with: ${param1}, ${param2}`);
        return param1 + param2;
    },

    /**
     - 僅供 Library 內部使用的本地函數範例 2
     - @param {string} param1
     - @param {boolean} param2
     - @param {object} param3
     */
    localFunction2: function (param1, param2, param3) {
        // ... 內部邏輯 ...
            console.log("Local function 2 called.");
    },

    // --- Ribbon Button 函數 ---
    /**
     - Ribbon 按鈕 'SyncSap' 的 Action 函數
     - @param {Xrm.FormContext} primaryControl - 通常傳入 formContext
     */
        SyncSap_action: function (primaryControl) {
            // primaryControl 在 Ribbon 中通常是 formContext
            const formContext = primaryControl;
            const accountId = formContext.data.entity.getId();
            alert(`準備同步 Account ID: ${accountId} 到 SAP。(Action)`);
            // 呼叫內部或外部函數執行實際同步邏輯
            // art.Account.localFunction1("Sync", accountId);
            // 或呼叫 Functional Library 的函數: commonUtils.callSyncApi(accountId);
        },

        /**
         - Ribbon 按鈕 'SyncSap' 的 Enable Rule 函數
         - @param {Xrm.FormContext} primaryControl - 通常傳入 formContext
         - @returns {boolean} - 回傳 true 表示啟用按鈕, false 表示禁用
         */
        SyncSap_enable: function (primaryControl) {
            const formContext = primaryControl;
            // 範例：只有當 'accountnumber' 欄位有值時才啟用按鈕
            const accountNumber = formContext.getAttribute("accountnumber")?.getValue();
            const canEnable = !!accountNumber; // 轉換成 boolean
            console.log(`SyncSap button enabled: ${canEnable}`);
            return canEnable;
        },
    };

    // 3. Global 全域變數 (以 Library 名稱 Account 作為前綴)
    var Account_globalVariable1 = "Initial Value";
    var Account_globalVariable2 = null;

    // 4. Global 具名函式 (以 Library 名稱 Account 作為前綴)
    // 這些函數通常被註冊到事件中 (如此處的 OnChange) 或被其他 Library 呼叫

    /**
     - 'address1_country' 欄位 OnChange 事件觸發的函數
     - @param {Xrm.Events.EventContext} executionContext 執行上下文
     */
    function Account_countryOnChange(executionContext) {
        const formContext = executionContext.getFormContext();
        const country = formContext.getAttribute("address1_country").getValue();
        alert(`國家變更為: ${country}. 需要執行相關邏輯。`);
        // 可能需要根據國家清空或設定省份/城市欄位
        // 可以呼叫 Library Object 內部的方法: art.Account.updateStateOptions(formContext, country);
        Account_globalVariable1 = `Country changed to ${country}`; // 更新全域變數範例
        console.log(`Global variable updated: ${Account_globalVariable1}`);
    }

    /**
     - 另一個可能被 onLoad 或其他地方呼叫的具名函數
     - @param {Xrm.Events.EventContext} executionContext 執行上下文
     */
    function Account_setupTypeOptions(executionContext) {
        const formContext = executionContext.getFormContext();
        // 根據某些條件設定 'accounttype' 欄位的選項
        console.log("Setting up account type options.");
    }
}
    // ... 其他 Account_ 開頭的具名函數 ...
```

- **規範重點回顧:**
    - **參數傳遞:** `art.Account.onLoad` 和 `Account_countryOnChange` 接收 `executionContext`。其他如 `art.Account.setupEditable`, `art.Account.SyncSap_action` 接收 `formContext`。
    - **Ribbon Button:** `SyncSap_action` 和 `SyncSap_enable` 遵循 `<BTN>_action`/`<BTN>_enable` 格式。
    - **命名與前綴:** Library 物件是 `art.Account`。全域變數和函數使用 `Account_` 前綴 (如 `Account_globalVariable1`, `Account_countryOnChange`)。Library 內部函數 (如 `localFunction1`) 不需前綴，因為它們透過 `art.Account.localFunction1` 訪問。
    - **內容排序:** 程式碼嚴格按照：`art` 命名空間 -> `art.Account` Library 物件 (含內部變數、函數、Ribbon 函數) -> `Account_` 全域變數 -> `Account_` 全域具名函數 的順序排列。
    - **常見函數:** `onLoad`, `setupEditable`, `setupMandatory`, `setupOnchange`, `setupOptions`, `setupFilters` 都在 `art.Account` 物件內定義，展示了建議的函數用途。