#### 📅 **Date**: 2025-03-13

#### 🔖 **Tags**: #CSharp #dotNET #BackEnd #InterviewQuestions

---

在 **C# 和 .NET** 中，`IQueryable` 和 `IEnumerable` 是 **LINQ 查詢的核心介面**，它們影響**資料處理的方式**，而 `ToList()` 則決定了**查詢何時執行**。

---

## **📍 1. IQueryable vs IEnumerable**

### **✅ 主要區別**

|          | **IQueryable**                      | **IEnumerable**               |
| -------- | ----------------------------------- | ----------------------------- |
| **定義**   | 適用於資料庫查詢（如 `EF Core`）               | 適用於記憶體內集合                     |
| **執行方式** | **延遲執行（Lazy Execution）**，轉換為 SQL 查詢 | **立即執行（Immediate Execution）** |
| **運行位置** | **資料庫（Server-Side）**                | **記憶體（Client-Side）**          |
| **適用場景** | **大數據查詢（最佳化 SQL）**                  | **小型集合處理（已載入記憶體）**            |

---

## **📍 2. `IQueryable`（SQL 優化查詢）**

`IQueryable<T>` **可將 LINQ 轉換為 SQL**，僅在**調用 `ToList()`、`FirstOrDefault()`、`Count()` 時才執行查詢**。

### **🔹 範例**

csharp

複製編輯

`using System; using System.Linq; using Microsoft.EntityFrameworkCore;  class Program {     static void Main() {         using (var db = new AppDbContext()) {             IQueryable<User> query = db.Users.Where(u => u.Age > 18); // SQL 尚未執行             var users = query.ToList(); // SQL 執行，結果載入記憶體         }     } }`

### **📌 `IQueryable` 的優勢**

✔ **查詢在 SQL Server 執行（效率高）**  
✔ **`WHERE`、`ORDER BY`、`SELECT` 會轉為 SQL**  
✔ **適用於 EF Core、NHibernate、Dapper**

🔹 **產生的 SQL**

sql

複製編輯

`SELECT * FROM Users WHERE Age > 18;`

---

## **📍 3. `IEnumerable`（記憶體內查詢）**

`IEnumerable<T>` **已經載入到記憶體**，所有 LINQ 運算都在 **C# 層面（Client-Side）執行**。

### **🔹 範例**

csharp

複製編輯

`IEnumerable<User> users = db.Users.ToList(); // SQL 立即執行 var filteredUsers = users.Where(u => u.Age > 18); // 在 C# 層面篩選`

### **📌 `IEnumerable` 的劣勢**

❌ **所有數據都載入記憶體，影響效能**  
❌ **無法轉換成 SQL 查詢**  
❌ **適用於小型集合，不適合大數據查詢**

🔹 **產生的 SQL**

sql

複製編輯

`SELECT * FROM Users; -- 載入所有資料`

📌 **篩選條件 (`.Where()`) 在 C# 層面執行，SQL 沒有過濾，造成效能問題！**

---

## **📍 4. `ToList()` 會發生什麼事？**

`ToList()` **會強制執行查詢，將 `IQueryable` 轉為 `List<T>`，資料載入記憶體**。

### **🔹 `ToList()` 影響查詢方式**

csharp

複製編輯

`// IQueryable（SQL 執行前） var query = db.Users.Where(u => u.Age > 18);  // SQL 會在此時執行 var usersList = query.ToList();` 

📌 **`ToList()` 會觸發 SQL 查詢，並將結果載入記憶體。**

---

## **📍 5. `IQueryable` vs `IEnumerable` vs `ToList()`**

||**IQueryable**|**IEnumerable**|**ToList()**|
|---|---|---|---|
|**運行位置**|**SQL Server（Server-Side）**|**記憶體（Client-Side）**|**記憶體（Client-Side）**|
|**查詢方式**|**延遲執行（Lazy）**|**立即執行（Immediate）**|**強制執行 SQL 查詢**|
|**效能**|**大數據最佳化**|**適用於小型集合**|**轉換成 `List<T>`，增加記憶體使用**|
|**適用場景**|**數據庫查詢（EF Core）**|**本地集合（List、Array）**|**獲取最終結果**|

---

## **📌 總結**

1️⃣ **`IQueryable` 讓查詢在 SQL Server 執行，減少記憶體使用，適合大數據查詢。**  
2️⃣ **`IEnumerable` 會將所有數據載入記憶體，適合小型集合，不適用於 SQL 查詢。**  
3️⃣ **`ToList()` 會立即執行查詢，將 `IQueryable` 轉換成 `List<T>`，適合獲取最終結果。**

---

🔥 **面試技巧** 如果面試官問：「`IQueryable` 和 `IEnumerable` 的區別？」  
✅ **回答**：「`IQueryable` 允許 SQL 優化，查詢在資料庫執行，`IEnumerable` 會將數據載入記憶體，適合小型集合。」