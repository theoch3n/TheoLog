#### 📅 **Date**: 2025-03-13

#### 🔖 **Tags**: #Basic #InterviewQuestions

---

在 **C#** 中，`async` / `await` 搭配 `Task` 主要用於 **非同步程式設計（Asynchronous Programming）**，能夠避免程式**阻塞（Blocking）**，提高應用程式的效能。

---

## **📍 1. `async` / `await` 的基本概念**

- **`async`**：表示這個方法是非同步的，可以使用 `await`。
- **`await`**：當程式執行到 `await`，會**讓出執行緒，等待 Task 完成後繼續執行**，而不會阻塞整個應用程式。
- **`Task`**：代表一個正在執行的非同步操作，可回傳 `Task<T>` 或 `Task`。

### **🔹 範例**

```csharp
using System;
using System.Threading.Tasks;
class Program {
	static async Task Main() {
		Console.WriteLine("開始");
		await DoWorkAsync();
		Console.WriteLine("結束");
	}
	static async Task DoWorkAsync() {
		Console.WriteLine("開始工作...");
		await Task.Delay(2000); // 模擬非同步等待
		Console.WriteLine("工作完成！");
	}
}
```

### **📌 執行結果**

```shell
開始
開始工作...
（2 秒後）
工作完成！
結束
```

📌 **關鍵點**
1. **`await Task.Delay(2000)` 不會阻塞主執行緒**，程式可以繼續執行其他工作。
2. **`DoWorkAsync()` 執行時，控制權讓回 `Main()`，不會影響後續程式的運作。**
3. **等 `Task.Delay(2000)` 完成後，程式才繼續執行 `工作完成！`。**

---

## **📍 2. `Task.Run()` vs `async/await`**

### **✅ `Task.Run()`（適用於 CPU 密集型工作）**

如果要在背景執行 CPU 密集型計算，可使用 `Task.Run()`：

```csharp
using System;
using System.Threading.Tasks;
class Program {
	static async Task Main() {
		Console.WriteLine("開始");
		int result = await Task.Run(() => Compute());
		Console.WriteLine($"計算結果：{result}");
		Console.WriteLine("結束");
	}
	static int Compute() {
		Console.WriteLine("執行 CPU 密集計算...");
		Task.Delay(3000).Wait(); // 模擬計算
		return 42;
	}
}
```

📌 **`Task.Run()` 會啟動新執行緒，適用於 CPU 密集型計算（如加密、壓縮、影像處理）。**

---

## **📍 3. `async void` vs `async Task`**

|**回傳類型**|**適用場景**|**可被 `await`**|**錯誤處理**|
|---|---|---|---|
|`async void`|**事件處理**|❌ **無法 `await`**|❌ **無法捕捉例外**|
|`async Task`|**非同步方法**|✅ **可 `await`**|✅ **可 `try-catch`**|

### **🔹 `async void`（事件處理）**

```csharp
async void Button_Click(object sender, EventArgs e) {
	await Task.Delay(1000);
	Console.WriteLine("按鈕被點擊");
}
```

📌 **`async void` 不能 `await`，也無法 `try-catch` 捕捉例外！**

---

### **🔹 `async Task`（推薦）**

```csharp
async Task FetchDataAsync() {
	try {
		await Task.Delay(2000);
		Console.WriteLine("數據獲取成功");
	} catch (Exception ex) {
		Console.WriteLine($"錯誤：{ex.Message}");
	}
}
```

📌 **`async Task` 可被 `await`，並支援 `try-catch` 捕捉錯誤。**

---

## **📍 4. `ConfigureAwait(false)` 提升效能**

在 **ASP.NET Core 或 Console 應用**，使用 `ConfigureAwait(false)` **避免回到主執行緒**，提高效能：

```csharp
async Task FetchDataAsync() {
	await Task.Delay(2000).ConfigureAwait(false);
	Console.WriteLine("後台完成數據獲取");
}
```

📌 **適用於 Web API，避免 UI Thread 等待，提高並發能力。**

---

## **📍 5. `await Task.WhenAll()` 與 `await Task.WhenAny()`**

### **✅ `Task.WhenAll()`（等待所有 Task 完成）**

```csharp
async Task RunTasksAsync() {
	Task t1 = Task.Delay(2000);
	Task t2 = Task.Delay(3000);
	await Task.WhenAll(t1, t2);
	Console.WriteLine("所有 Task 完成！");
}
```

📌 **當所有 Task 完成後才會繼續執行。**

---

### **✅ `Task.WhenAny()`（等待任一 Task 完成）**

```csharp
async Task RunTasksAsync() {
	Task t1 = Task.Delay(2000);
	Task t2 = Task.Delay(3000);
	await Task.WhenAny(t1, t2);
	Console.WriteLine("某個 Task 先完成！");
}
```

📌 **當任意一個 Task 完成後，程式就會繼續執行。**

---

## **💡 總結**

|**關鍵字**|**用途**|**適用場景**|
|---|---|---|
|`async`|宣告非同步方法|讓方法支援 `await`|
|`await`|等待非同步結果|**不會阻塞執行緒**，適用於 I/O 操作|
|`Task`|代表異步操作|`Task<T>` 可回傳結果|
|`Task.Run()`|執行背景工作|**CPU 密集型工作（如加密、壓縮）**|
|`async void`|事件處理|**不能 `await`，無法捕捉錯誤**|
|`async Task`|一般非同步方法|**可 `await`，可 `try-catch`**|
|`ConfigureAwait(false)`|避免回到主執行緒|**ASP.NET Core、Console 最佳化**|
|`Task.WhenAll()`|同時等待多個 Task|**適合批量 API 請求**|
|`Task.WhenAny()`|只等待最快的 Task|**適合多個請求取最先完成者**|

---

🔥 **面試技巧** 如果面試官問：「`async` / `await` 如何運作？」  
✅ **回答**：「當程式執行到 `await`，會讓出執行緒，讓其他工作繼續執行，等 `Task` 完成後再回來執行剩下的程式碼，從而提高應用程式的效能。」