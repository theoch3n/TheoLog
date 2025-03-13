#### 📅 **Date**: 2025-03-13

#### 🔖 **Tags**: #MVC #DesignPattern #InterviewQuestions

---

DI（**Dependency Injection，依賴注入**）是一種 **設計模式**，主要用於 **解耦（Decoupling）** 物件之間的依賴，使系統更容易測試、維護和擴展。

在 **.NET Core** 和 **ASP.NET Core**，DI 是內建的 **IoC（Inversion of Control, 控制反轉）** 機制。

---

## **📍 1. 為什麼需要 DI？**

### **🔹 傳統方式（無 DI，緊耦合）**

csharp

複製編輯

`public class OrderService {     private EmailService _emailService = new EmailService(); // 直接建立依賴      public void ProcessOrder() {         _emailService.SendEmail("訂單已處理");     } }`

📌 **問題**

- `OrderService` **強依賴** `EmailService`
- **難以單元測試（Unit Test）**，因為 `EmailService` 不能輕易替換
- **不易擴展**，如果要更換 `SMSService`，需要修改 `OrderService` 的程式碼

---

## **📍 2. 使用 DI 進行解耦**

### **🔹 DI 改寫（依賴介面，易測試、易擴展）**

csharp

複製編輯

`public interface INotificationService {     void Send(string message); }  public class EmailService : INotificationService {     public void Send(string message) {         Console.WriteLine($"發送 Email: {message}");     } }  public class OrderService {     private readonly INotificationService _notificationService;      public OrderService(INotificationService notificationService) { // 依賴注入         _notificationService = notificationService;     }      public void ProcessOrder() {         _notificationService.Send("訂單已處理");     } }`

📌 **優勢**

- **解耦（Decoupling）**，`OrderService` **不再直接依賴 `EmailService`**
- **易測試（Testability）**，可傳入 `Mock` 物件進行單元測試
- **可替換（Extensible）**，可以改為 `SMSService` 或其他通知方式

---

## **📍 3. ASP.NET Core 內建 DI**

ASP.NET Core 提供內建的 **DI 容器**，可透過 `ConfigureServices` 來註冊依賴。

### **🔹 註冊 DI（在 `Startup.cs` 或 `Program.cs`）**

csharp

複製編輯

`var builder = WebApplication.CreateBuilder(args);  builder.Services.AddScoped<INotificationService, EmailService>(); // 註冊 DI  var app = builder.Build();`

### **🔹 使用 DI（在 Controller 中）**

csharp

複製編輯

`public class OrderController : ControllerBase {     private readonly INotificationService _notificationService;      public OrderController(INotificationService notificationService) {         _notificationService = notificationService;     }      [HttpPost]     public IActionResult CreateOrder() {         _notificationService.Send("訂單已建立");         return Ok("訂單處理完成");     } }`

📌 **優勢**

- **ASP.NET Core 會自動解析 `INotificationService`**
- **Controller 不需要自己 `new` 物件，減少耦合**

---

## **📍 4. DI 生命週期（Service Lifetime）**

在 **ASP.NET Core**，DI 物件可以有 **不同的生命週期（Lifetime）**：

|**生命週期**|**說明**|**適用場景**|
|---|---|---|
|**Transient**|每次請求都建立新實例|**短期物件（如計算服務）**|
|**Scoped**|每個 **HTTP 請求** 共用一個實例|**用於 API 請求期間的服務**|
|**Singleton**|整個應用程式 **共用一個實例**|**配置、全域快取等服務**|

📌 **註冊方式**

csharp

複製編輯

`builder.Services.AddTransient<IMyService, MyService>(); // 每次請求新建 builder.Services.AddScoped<IMyService, MyService>();    // 每個請求共用 builder.Services.AddSingleton<IMyService, MyService>(); // 應用程式共用`

---

## **📌 總結**

✅ **DI 讓物件之間的依賴變得可控，提升可測試性和可擴展性**  
✅ **ASP.NET Core 內建 DI，支援 `Transient`、`Scoped`、`Singleton` 註冊方式**  
✅ **降低耦合，提高可維護性，避免 `new` 物件造成的硬性依賴**

---

🔥 **面試技巧** 如果面試官問：「什麼是 DI？為什麼要使用 DI？」  
✅ **回答重點**： 1️⃣ **DI（依賴注入）是一種設計模式，透過注入方式取代 `new`，降低耦合**  
2️⃣ **DI 讓程式更易測試、更易擴展（例如從 `EmailService` 換成 `SMSService`）**  
3️⃣ **ASP.NET Core 內建 DI，支援 `Transient`、`Scoped`、`Singleton`**