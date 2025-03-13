#### 📅 **Date**: 2025-03-13

#### 🔖 **Tags**: #Basic #InterviewQuestions

---

當 **主執行緒（Main Thread）** 執行 **`async` 方法** 並遇到 **`await`**，會**發生什麼？它會卡住嗎？還是繼續執行其他任務？**

這是 C# 和 .NET 面試的經典問題，考察你對 **非同步（Asynchronous）** 和 **事件驅動架構（Event Loop）** 的理解。

---

## **📍 1. `async` / `await` 的基本概念**

- **`async`**：表示該方法是「非同步」的，可能會執行 `await` 操作。
- **`await`**：當遇到 `await` 時，程式會「暫時讓出主執行緒」，但不會阻塞主執行緒。

```csharp
async Task DoWork() {
	Console.WriteLine("開始工作");
	await Task.Delay(2000); // 模擬耗時操作
	Console.WriteLine("工作完成");
}
```

**流程：** 
1. **DoWork() 開始執行**，輸出 `"開始工作"`。  
2. **遇到 `await Task.Delay(2000)`**：

- **主執行緒（Main Thread）讓出控制權**，執行其他工作
- `Task.Delay(2000)` 在 **背景執行緒** 非同步等待 **2 秒後，背景工作完成**，回到 **主執行緒**，執行 `"工作完成"`。

---

## **📍 2. `Main Thread` 會被 `await` 阻塞嗎？**

讓我們測試以下 `Main()` 方法：

```csharp
static async Task Main() {
	Console.WriteLine("開始");
	await DoWork();
	Console.WriteLine("結束");
}
```

### **📌 執行結果**

```shell
開始
開始工作
（2 秒後）
工作完成
結束
```

### **📌 發生了什麼？**

- **`Main Thread` 執行 `"開始"`**
- **`DoWork()` 執行 `"開始工作"`**
- **遇到 `await Task.Delay(2000)`，`Main Thread` 讓出 CPU**
- **2 秒後，背景執行緒完成，繼續執行 `"工作完成"`**
- **執行 `"結束"`**

👉 **重點**：`Main Thread` **不會被阻塞，而是會去執行其他工作！**

---

## **📍 3. `await` 與 `ConfigureAwait(false)` 的影響**

### **🔹 預設情況**

`await` 之後的程式碼會**回到原本的執行緒（Synchronization Context）**：

```csharp
async Task DoWork() {
	Console.WriteLine($"開始工作：執行緒 {Thread.CurrentThread.ManagedThreadId}");
	await Task.Delay(2000);
	Console.WriteLine($"工作完成：執行緒 {Thread.CurrentThread.ManagedThreadId}");
}
```

**可能的輸出：**

```shell
開始工作：執行緒 1
（2 秒後）
工作完成：執行緒 1
```

👉 **結果**：執行緒 `1`（主執行緒）負責執行 `"開始工作"` 和 `"工作完成"`。

---

### **🔹 `ConfigureAwait(false)`**

使用 `ConfigureAwait(false)` **避免回到主執行緒**，可以提升效能：

```csharp
async Task DoWork() {
	Console.WriteLine($"開始工作：執行緒 {Thread.CurrentThread.ManagedThreadId}");
	await Task.Delay(2000).ConfigureAwait(false);
	Console.WriteLine($"工作完成：執行緒 {Thread.CurrentThread.ManagedThreadId}");
}
```

**可能的輸出：**

```shell
開始工作：執行緒 1
（2 秒後）
工作完成：執行緒 5
```

👉 **結果**：`工作完成` 在背景執行緒（如 `5`）執行，而不回到主執行緒。

---

## **📍 4. `Main Thread` vs `UI Thread`**

如果是 **`Console App`**，`Main Thread` **不一定是 UI 執行緒**，而 `await` 之後的程式碼可能會在背景執行緒執行。

但如果是 **`WinForms` / `WPF` 應用程式**，`Main Thread` **就是 UI 執行緒**：

- **沒有 `ConfigureAwait(false)`**：會回到 UI 執行緒，確保 UI 可操作。
- **加上 `ConfigureAwait(false)`**：可能在背景執行緒執行，避免影響 UI 效能。

---

## **📍 5. 面試延伸問題**

### **Q1: `async void` 和 `async Task` 有什麼區別？**

| |`async void`|`async Task`|
|---|---|---|
|**回傳值**|無回傳值|回傳 `Task`|
|**例外處理**|**無法 `try-catch` 捕捉異常**|可用 `await` 正確捕捉異常|
|**適用場景**|**事件處理（如 Button Click）**|一般 `async` 方法|

**例子：**

```csharp
async void Button_Click(object sender, EventArgs e) {  // 只能用於事件
	await DoWork();
}
```

```csharp
async Task SomeMethodAsync() {  // 可用 await
	await DoWork();
}
```

---

### **Q2: `Task.Run()` vs `async/await` 有什麼不同？**

| |`Task.Run()`|`async/await`|
|---|---|---|
|**使用場景**|**CPU 密集型** 工作|**I/O 密集型**（如 API 呼叫）|
|**執行緒**|**開新執行緒** 執行|只要 `await`，主執行緒可繼續執行|

**範例：**

```csharp
// 使用 Task.Run() 執行 CPU 密集型工作
Task.Run(() => {
	for (int i = 0; i < 1000000; i++) {
		Math.Sqrt(i);
	}
});
```

```csharp
// 使用 async/await 執行 I/O 操作
await HttpClient.GetStringAsync("https://example.com");
```

---

### **Q3: `await Task.Delay(0)` 會發生什麼？**

```csharp
await Task.Delay(0);
```

- `Task.Delay(0)` **不會真正等待**，但**會讓出主執行緒**，類似 `Thread.Yield()`。
- 可能的用途：讓目前的 `Thread` 交出 CPU 給其他等待中的 `Task`。

---

## **📌 總結**

1. **`async` 方法** 碰到 **`await`**，會讓出 `Main Thread` 執行其他工作，而非阻塞主執行緒。  
2. **`ConfigureAwait(false)`** 可避免回到 `Main Thread`，提升效能（適用於 `Console` 和 `ASP.NET`）。  
3. **`async void` 只能用於事件處理**，其他情境應該用 `async Task`。  
4. **`Task.Run()` 用於 CPU 密集型工作**，`async/await` 適用於 I/O 操作。

---

🔥 **面試技巧**：當面試官問 **「`await` 會不會阻塞 `Main Thread`？」**  
✅ **正確回答**：  
**「不會，`await` 會讓出 `Main Thread` 給其他任務，當 `Task` 完成後再回到 `Main Thread` 繼續執行。」**