#### 📅 **Date**: 2025-03-12

#### 🔖 **Tags**: `#MVC` `#DesignPattern` `#InterviewQuestions`

---

MVC（**Model-View-Controller**）是一種 **軟體架構設計模式**，主要用於分離程式的 **數據邏輯（Model）**、**使用者介面（View）** 和 **控制邏輯（Controller）**，以提升 **可維護性、可測試性**，並讓開發更具模組化。

---

## **📍 MVC 架構概念**

- **Model（模型）**：負責處理數據和商業邏輯。  
- **View（視圖）**：負責顯示 UI，與使用者互動。  
- **Controller（控制器）**：負責接收請求、調用 Model 並回傳 View。

![MVC 流程](https://upload.wikimedia.org/wikipedia/commons/thumb/a/a0/MVC-Process.svg/1200px-MVC-Process.svg.png)

### **📌 MVC 的工作流程**

1. **使用者** 透過瀏覽器發送請求。  
2. **Controller** 接收請求，執行業務邏輯（可能會存取 Model）。  
3. **Model** 負責數據處理，並將結果傳回 Controller。  
4. **Controller** 將數據傳給 **View**，並回應給使用者。

---

## **📍 MVC 在 ASP.NET Core 中的應用**

ASP.NET Core MVC 是基於 MVC 設計模式的 Web 應用程式框架，典型的 MVC 結構包含：

📁 `Controllers`（控制器）  
📁 `Models`（模型）  
📁 `Views`（視圖）

#### **📝 Controller 範例**

```csharp
public class HomeController : Controller {
	public IActionResult Index() {
		return View();
	}
}
```

> **說明**：當使用者訪問 `/Home/Index`，此 `Controller` 會回傳 `View()`。

---

#### **📝 Model 範例**

```csharp
public class Product {
	public int Id { get; set; }
	public string Name { get; set; }
	public decimal Price { get; set; }
}
```

> **說明**：`Product` Model 定義了產品的結構。

---

#### **📝 View 範例（Razor）**

```cshtml
@model Product
<h1>@Model.Name</h1>
<p>Price: $@Model.Price</p>
```

> **說明**：View 透過 `@Model` 來渲染數據。

---

## **📍 為什麼要用 MVC？（面試重點）**

- **分離關心點（Separation of Concerns）**，使代碼更乾淨  
- **提升可維護性**，修改某部分不影響整體  
- **增強可測試性**，適用於單元測試  
- **易於團隊開發**，前端、後端可獨立作業

---

## **📍 面試可能會問的延伸問題**

1. **MVC 和 MVVM（Model-View-ViewModel）有什麼不同？**
    - `MVVM` 多了一層 `ViewModel`，主要用於雙向數據綁定，適合前端框架（如 React、Vue.js、Angular）。
2. **ASP.NET Core MVC 和 Razor Pages 的區別？**
    - `MVC` 適合大型 Web 應用，有明確的 `Controller`。
    - `Razor Pages` 更簡單，適合小型應用，View 和業務邏輯在同一個 `.cshtml.cs` 檔案內。
3. **可以不用 Controller 直接渲染 View 嗎？**
    - 可以使用 `ViewComponent` 或 `Razor Pages`，但 MVC 架構鼓勵使用 `Controller` 來管理請求。

---

## **💡 總結**

MVC 是一種**設計模式**，主要用於**分離應用程式的業務邏輯與 UI**，在 ASP.NET Core、Java Spring、Django 等 Web 框架中廣泛使用。透過 MVC，可以讓開發更模組化、可維護性更高、團隊合作更高效。