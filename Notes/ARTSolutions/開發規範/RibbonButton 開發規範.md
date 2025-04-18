#### 📅 **Date**: 2025-04-18

#### 🔖 **Tags**: #RibbonButton #PowerApps #Dynamics365

---

1. **每個按鈕限制一個 Action 和一個 Enable Rule 函數：**
    - **說明：** 在定義 Ribbon Button 時，其「動作 (Action)」(點擊後執行的邏輯) 和「啟用規則 (Enable Rule)」(決定按鈕是否可用/可見的邏輯) 應該各自只對應到一個 JavaScript 函數。
    - **目的：** 保持 Ribbon Button 設定的簡潔和清晰。將按鈕的「做什麼」和「何時能做」的邏輯分別封裝在單一、明確的函數中，便於查找、理解和維護。
2. **呼叫 Functional Library 需先調用 `load` 函數：**
    - **說明：** 如果 Ribbon Button 的 Action 或 Enable Rule 函數需要使用來自共享的「功能性函式庫 (Functional Library)」中的代碼，那麼在呼叫該函式庫的其他函數之前，必須先呼叫該函式庫內一個名為 `load` 的函數（此函數可能是空的）。
    - **目的：** 這是一個特定的團隊約定 (#Daniel)。其目的很可能是為了確保在 Ribbon 的執行環境下，目標 Functional Library 已經被正確載入和初始化，之後才能安全地使用其提供的功能。這可能是一種處理腳本載入順序或依賴關係的機制。
3. **限制呼叫單一主 Action 函數，多步驟需封裝：**
    - **說明：** Ribbon Button 的 Command 設定中，只能指定一個主要的 JavaScript 函數作為其 Action 的進入點。如果一個按鈕點擊需要執行多個不同的邏輯操作，不應該在 Ribbon 設定中嘗試鏈接多個函數。相反地，應該建立一個「按鈕專屬的 Action 函數 (BTN Action)」，並將所有需要執行的步驟按順序在這個函數內部進行呼叫。
    - **目的：** 提升程式碼的組織性和流程控制能力。將複雜的多步驟邏輯封裝在單一的 JavaScript 函數中，可以更好地管理執行順序、進行錯誤處理、實現條件判斷，並使除錯更加容易。同時，這也保持了 Ribbon XML 設定的簡潔性，將複雜的業務流程放在更適合的 JavaScript 層處理。
