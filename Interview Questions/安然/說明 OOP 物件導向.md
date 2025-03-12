#### 📅 **Date**: 2025-03-12

#### 🔖 **Tags**: #Basic #InterviewQuestions

---

OOP（**Object-Oriented Programming**）是一種**程式設計範式**，透過**物件（Object）**來封裝**數據（Attributes）**與**行為（Methods）**，讓程式結構更具模組化、可維護性更高、易於擴展。

---

## **📍 OOP 的四大特性**

OOP 有 **四大核心特性**，這也是面試常考的重點：

|特性|說明|例子|
|---|---|---|
|**封裝（Encapsulation）**|將數據與行為封裝在物件內部，外部無法直接存取|`private`、`public`、`getter/setter`|
|**繼承（Inheritance）**|子類別（Subclass）可以繼承父類別（Superclass）的屬性與方法|`class Car : Vehicle {}`|
|**多型（Polymorphism）**|同一個方法可在不同類別中有不同實作|方法重載（Overloading）與方法覆寫（Overriding）|
|**抽象（Abstraction）**|隱藏不必要的細節，只對外暴露必要的功能|`abstract class`、`interface`|

---

## **📍 OOP 在 C# 中的應用**

### **1. 封裝（Encapsulation）**

```csharp
public class Person {
	private string name; // 私有屬性，無法直接存取
	public string GetName() {  // 封裝：透過方法存取
		return name;
	}
	public void SetName(string newName) {
		name = newName;
	}
}
```

**👉 目的：** 限制 `name` 直接被外部修改，提升安全性。

---

### **2️. 繼承（Inheritance）**

```csharp
public class Animal {
	public string Name { get; set; }
	public void Speak() {
		Console.WriteLine("Animal Sound");
	}
}

public class Dog : Animal {  // Dog 繼承 Animal
	public void Bark() {
		Console.WriteLine("Woof! Woof!");
	}
}
```

**👉 目的：** `Dog` 不用重複定義 `Name` 和 `Speak()`，可直接使用 `Animal` 的功能。

---

### **3️. 多型（Polymorphism）**

```csharp
public class Animal {
	public virtual void MakeSound() {
		Console.WriteLine("Some animal sound...");
	}
}

public class Cat : Animal {
	public override void MakeSound() {  // 方法覆寫（Overriding）
		Console.WriteLine("Meow");
	}
}

public class Dog : Animal {
	public override void MakeSound() {
		Console.WriteLine("Bark");
	}
}
```

**👉 目的：** `Animal` 的 `MakeSound()` 方法可被 `Cat` 和 `Dog` 重新定義。

---

### **4️. 抽象（Abstraction）**

```csharp
public abstract class Vehicle {
	public abstract void Move();  // 抽象方法，子類別必須實作
}

public class Car : Vehicle {
	public override void Move() {
		Console.WriteLine("Car is driving...");
	}
}
```

**👉 目的：** `Vehicle` 定義一個 `Move()` 方法，但具體實作交給 `Car` 來決定。

---

## **📍 OOP 的優勢（為什麼要用 OOP？）**

- **提升程式的可讀性與可維護性**（Encapsulation）  
- **減少重複代碼（Code Reuse）**（Inheritance）  
- **更靈活的程式結構（Extensibility）**（Polymorphism）  
- **提高程式的抽象化，減少耦合**（Abstraction）

---

## **📍 面試常見 OOP 問題**

1. **什麼是物件？什麼是類別？**
    - `類別（Class）` 是**模版**，`物件（Object）` 是**類別的實例**。
2. **多型有哪兩種形式？**
    - **方法重載（Overloading）**：同一個類別內，相同方法名稱但參數不同。
    - **方法覆寫（Overriding）**：子類別重新定義父類別的方法（必須用 `virtual` 和 `override`）。
3. **抽象類別（Abstract Class）和介面（Interface）有什麼不同？**
    - **抽象類別**：可以有 **已實作** 和 **未實作** 的方法，適合繼承用。
    - **介面**：只能定義**方法簽名**，所有方法都必須由實作類別定義，適合多重實作。
4. **OOP 和 FP（函數式程式設計，Functional Programming）的差異？**
    - **OOP** 強調**物件、狀態與行為**，適合模組化設計。
    - **FP** 強調**不可變性、純函數**，適合數據驅動開發（如 LINQ）。

---
## **💡 總結**

OOP 是當今主流的程式設計方式，透過 **封裝、繼承、多型、抽象**，提升可維護性、擴展性與可讀性。