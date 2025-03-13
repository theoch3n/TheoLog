#### 📅 **Date**: 2025-03-13

#### 🔖 **Tags**: #SQL #BackEnd #InterviewQuestions

---

在關聯式資料庫中，「多對多（Many-to-Many, M:N）」是一種常見的關係模型，表示**A 表中的多個記錄可以對應 B 表的多個記錄，反之亦然**。這通常透過**中介表（Join Table）**來實作。

---

## **📍 1. 多對多關係的概念**

### **✅ 什麼是多對多關係？**

- **A 表的一筆記錄可以對應到 B 表的多筆記錄**
- **B 表的一筆記錄可以對應到 A 表的多筆記錄**
- **需要透過「中介表」來建立連結**

### **🎯 範例**

📌 **學生（Students）與課程（Courses）關係**

> **「一個學生可以選修多門課程，一門課程可以有多個學生」**

|**學生表（Students）**||**課程表（Courses）**|
|---|---|---|
|`id`（PK）|🔗|`id`（PK）|
|`name`||`title`|

📌 **解決方案**：建立**中介表（student_course）**

|**中介表（student_course）**||
|---|---|
|`student_id`（FK）|🔗 `students.id`|
|`course_id`（FK）|🔗 `courses.id`|
|**PK（主鍵）**|`(student_id, course_id)`|

---

## **📍 2. 多對多關係的 SQL 設計**

### **✅ 建立表格**

sql

複製編輯

`-- 建立學生表 CREATE TABLE students (     id INT PRIMARY KEY AUTO_INCREMENT,     name VARCHAR(50) NOT NULL );  -- 建立課程表 CREATE TABLE courses (     id INT PRIMARY KEY AUTO_INCREMENT,     title VARCHAR(100) NOT NULL );  -- 建立中介表（關聯學生與課程） CREATE TABLE student_course (     student_id INT,     course_id INT,     PRIMARY KEY (student_id, course_id),     FOREIGN KEY (student_id) REFERENCES students(id) ON DELETE CASCADE,     FOREIGN KEY (course_id) REFERENCES courses(id) ON DELETE CASCADE );`

📌 **關鍵設計**

- **`PRIMARY KEY (student_id, course_id)`**：確保不會有重複記錄
- **`FOREIGN KEY` 約束**：確保 `student_id` & `course_id` 參考正確的表
- **`ON DELETE CASCADE`**：當 `students` 或 `courses` 中的記錄刪除時，自動刪除中介表的對應記錄

---

## **📍 3. 操作多對多關係**

### **✅ 插入資料**

sql

複製編輯

`-- 插入學生 INSERT INTO students (name) VALUES ('Alice'), ('Bob');  -- 插入課程 INSERT INTO courses (title) VALUES ('Math'), ('Science');  -- Alice 選修 Math（學生 ID = 1, 課程 ID = 1） INSERT INTO student_course (student_id, course_id) VALUES (1, 1);  -- Bob 選修 Math 和 Science（學生 ID = 2, 課程 ID = 1 & 2） INSERT INTO student_course (student_id, course_id) VALUES (2, 1), (2, 2);`

---

### **✅ 查詢某學生選修的課程**

sql

複製編輯

`SELECT s.name, c.title FROM students s JOIN student_course sc ON s.id = sc.student_id JOIN courses c ON sc.course_id = c.id WHERE s.id = 2;`

📌 **解釋**：

1. **`JOIN student_course`** → 找到學生與課程的關聯
2. **`JOIN courses`** → 取得對應的課程名稱

**結果**

javascript

複製編輯

`Bob | Math Bob | Science`

---

### **✅ 查詢某課程的學生**

sql

複製編輯

`SELECT c.title, s.name FROM courses c JOIN student_course sc ON c.id = sc.course_id JOIN students s ON sc.student_id = s.id WHERE c.id = 1;`

📌 **解釋**：

- 找出所有選修 `Math（id = 1）` 的學生

**結果**

javascript

複製編輯

`Math | Alice Math | Bob`

---

### **✅ 刪除某學生時，同步刪除關聯**

sql

複製編輯

`DELETE FROM students WHERE id = 1;`

📌 **結果**

- `students.id = 1` 被刪除
- **`student_course` 中相關的 `student_id = 1` 也會自動刪除（因為 `ON DELETE CASCADE`）**

---

## **📍 4. 多對多與 Index（索引）**

### **✅ 為什麼多對多關係要加索引？**

當數據量大時，查詢 `JOIN` 可能會變慢，透過**索引（Index）**可以提升查詢效能。

📌 **為中介表加索引**

sql

複製編輯

`CREATE INDEX idx_student_course_student ON student_course(student_id); CREATE INDEX idx_student_course_course ON student_course(course_id);`

🔹 **效果**

- **加速 `WHERE student_id = ?` 的查詢**
- **加速 `WHERE course_id = ?` 的查詢**

---

## **📌 總結**

|**重點**|**說明**|
|---|---|
|**多對多關係的本質**|**需要中介表來建立關聯**|
|**設計方式**|`A 表` ↔ `中介表` ↔ `B 表`|
|**中介表的主鍵**|`PRIMARY KEY (A_id, B_id)`|
|**使用索引提升效能**|`INDEX (A_id)`、`INDEX (B_id)`|
|**ON DELETE CASCADE**|**當主表刪除時，自動刪除關聯記錄**|

---

🔥 **面試技巧** 如果面試官問：「多對多關係如何設計？」  
✅ **回答**：「多對多關係需要建立 **中介表**，將兩個表的 `id` 作為外鍵，並設置 `PRIMARY KEY(student_id, course_id)`，以確保數據唯一性。可透過 `JOIN` 查詢資料，並使用索引優化查詢效能。」