#### 📅 **Date**: 2025-03-12

#### 🔖 **Tags**:  #Basic #CSharp #Java #BackEnd #InterviewQuestions

---

在不同的程式語言中，`Array` 和 `List` 都是常見的**集合類型（Collection Types）**，但它們有不同的特性和應用場景。

---

## **📍 1. Array vs List 的主要區別**

| **比較項目**            | **Array（陣列）**        | **List（列表）**                  |
| ------------------- | -------------------- | ----------------------------- |
| **大小（Size）**        | **固定長度（宣告後不可變）**     | **可變長度（動態增減元素）**              |
| **記憶體管理**           | **連續記憶體分配（高效但難以擴展）** | **非連續記憶體（擴展靈活但有額外開銷）**        |
| **新增/刪除**           | **不可變長，刪除困難**        | **支援 `Add()`、`Remove()`，可變長** |
| **存取方式**            | **索引（O(1)）**         | **索引（O(1)）**                  |
| **效能（Performance）** | **讀取快，增刪慢**          | **讀取快，增刪較快**                  |
| **適用場景**            | **固定長度的數據（如矩陣、快取）**  | **需要動態增刪的資料（如清單、佇列）**         |

---

## **📍 2. C# 中 `Array` vs `List<T>`**

### **✅ `Array`（固定長度）**

csharp

複製編輯

`int[] numbers = new int[3] { 1, 2, 3 }; Console.WriteLine(numbers[0]); // 輸出 1`

📌 **限制**

- **無法動態新增/刪除元素**
- **若要新增元素，必須建立新陣列**

---

### **✅ `List<T>`（可變長度）**

csharp

複製編輯

`using System; using System.Collections.Generic;  List<int> numbers = new List<int> { 1, 2, 3 }; numbers.Add(4); // 新增元素 numbers.Remove(2); // 刪除元素 Console.WriteLine(numbers.Count); // 3`

📌 **優勢**

- **可動態調整大小**
- **提供豐富的方法，如 `Add()`、`Remove()`、`Contains()`**

---

## **📍 3. Java 中 `Array` vs `ArrayList`**

### **✅ `Array`（固定長度）**

java

複製編輯

`int[] numbers = new int[3]; numbers[0] = 1;`

📌 **無法動態變更大小**

---

### **✅ `ArrayList`（可變長度）**

java

複製編輯

`import java.util.ArrayList;  ArrayList<Integer> numbers = new ArrayList<>(); numbers.add(1); numbers.add(2); numbers.remove(0); System.out.println(numbers.size()); // 1`

📌 **適合頻繁增刪的場景**

---

## **📍 4. JavaScript 中 `Array`**

JavaScript **沒有 List**，但 `Array` 本身是**動態可變長度**的：

javascript

複製編輯

`let arr = [1, 2, 3]; arr.push(4);  // 新增 arr.splice(1, 1); // 刪除索引 1 的元素 console.log(arr); // [1, 3, 4]`

📌 **JavaScript 的 `Array` 具有 `List` 的特性**

---

## **📌 總結**

|**語言**|**固定長度（Array）**|**可變長度（List）**|
|---|---|---|
|**C#**|`int[]`|`List<T>`|
|**Java**|`int[]`|`ArrayList<T>`|
|**JavaScript**|`Array`（但可變）|無需額外類別|

---

🔥 **面試技巧** 如果面試官問：「`Array` 和 `List` 的差異？」  
✅ **回答**：「`Array` 是**固定大小**的，適合快速存取，但不易擴展；`List` 則是**可變大小**的，適合頻繁增刪元素。」