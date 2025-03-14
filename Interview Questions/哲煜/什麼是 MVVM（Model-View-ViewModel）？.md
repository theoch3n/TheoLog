#### 📅 **Date**: 2025-03-14

#### 🔖 **Tags**: #MVVM #DesignPattern #InterviewQuestions

---

MVVM（**Model-View-ViewModel**）是一種 **前端架構設計模式**，主要用於 **分離 UI（View）與業務邏輯（Model）**，並透過 **ViewModel** 負責數據綁定，使 UI 和數據同步。

MVVM 主要應用於 **WPF、Xamarin、Vue.js、Angular、React（MVP 類似概念）**，特別適用於**雙向數據綁定（Two-Way Data Binding）**的場景。

---

## **📍 1. MVVM 結構**

|**層級**|**負責內容**|**範例**|
|---|---|---|
|**Model（模型層）**|**數據層**，存取資料庫、API|`UserModel.cs`|
|**View（視圖層）**|**UI 層**，顯示數據|`XAML, HTML`|
|**ViewModel（視圖模型層）**|**負責數據綁定、邏輯處理**|`UserViewModel.cs`|

📌 **View 只負責 UI，ViewModel 負責邏輯，Model 負責數據**  
📌 **View 不直接與 Model 互動，而是透過 ViewModel 進行溝通**

---

## **📍 2. MVVM 在 WPF / .NET 中的實作**

### **🔹 Model（數據層，負責存儲數據）**

```csharp
public class UserModel {     
	public string Name { get; set; }     
	public int Age { get; set; } 
}
```

---

### **🔹 ViewModel（邏輯層，負責處理 UI 綁定）**

```csharp
using System.ComponentModel;  
public class UserViewModel : INotifyPropertyChanged {     
	private UserModel _user;          
	public UserModel User {         
		get => _user;         
		set {             
			_user = value;             
			OnPropertyChanged("User");         
		}     
	}      
	public UserViewModel() {         
		User = new UserModel { Name = "Alice", Age = 25 };     
	}      
	public event PropertyChangedEventHandler PropertyChanged;     
	protected void OnPropertyChanged(string propertyName) {         
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));     } 
}
```

📌 **`INotifyPropertyChanged` 可讓 View 自動更新 UI**  
📌 **`OnPropertyChanged("User")` 讓數據變動時通知 View**

---

### **🔹 View（UI 層，負責顯示數據）**

```xml
<Window x:Class="MainWindow" xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
	<Window.DataContext>         
		<local:UserViewModel/>     
	</Window.DataContext>      
	<Grid>         
		<TextBox Text="{Binding User.Name, UpdateSourceTrigger=PropertyChanged}" />
		<TextBlock Text="{Binding User.Age}" />     
	</Grid> 
</Window>
```

📌 **`{Binding User.Name}`** → 讓 `TextBox` 與 `User.Name` 進行 **雙向綁定**  
📌 **`UpdateSourceTrigger=PropertyChanged`** → 讓 UI 變動時，ViewModel 也同步更新

---

## **📍 3. MVVM 的優勢**

✅ **分離 UI 與邏輯**（提升可讀性）  
✅ **支援數據綁定**（避免手動更新 UI）  
✅ **提高可測試性（Unit Test）**

---

## **💡 總結**

1. **Model**（數據層）→ 負責數據管理（資料庫、API）  
2. **ViewModel**（邏輯層）→ **處理 UI 綁定，觸發 PropertyChanged**  
3. **View**（視圖層）→ 負責 UI 顯示，透過 `Binding` 綁定 ViewModel

---

🔥 **面試技巧** 如果面試官問：「MVVM 的核心概念是什麼？」  
✅ **回答**：「MVVM 透過 `ViewModel` 來**解耦 UI（View）與數據（Model）**，透過 **數據綁定（Data Binding）** 讓 UI 和業務邏輯保持同步，提高可維護性與測試性。」