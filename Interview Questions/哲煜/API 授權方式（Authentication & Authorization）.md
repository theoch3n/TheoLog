#### 📅 **Date**: 2025-03-14

#### 🔖 **Tags**: #Basic #BackEnd #InterviewQuestions

---

API 授權是確保 **API 安全性** 的關鍵，常見的授權方式包括 **API Key、OAuth、JWT、Basic Auth** 等，每種方式適用於不同的場景。

---

## **📍 1. API 授權 vs 認證**

|**術語**|**定義**|
|---|---|
|**Authentication（身份驗證）**|驗證 **請求者是誰**（Who are you?）|
|**Authorization（授權）**|驗證 **請求者是否有權存取資源**（Are you allowed?）|

📌 **例如**
- **Authentication**：使用者登入時提供帳號 & 密碼
- **Authorization**：只有管理員能存取 `admin` API

---

## **📍 2. API 常見授權方式**

|**授權方式**|**適用場景**|**安全性**|
|---|---|---|
|**API Key**|內部 API、無需用戶身份驗證|⭐⭐⭐（需保護 Key）|
|**Basic Auth**|測試 API 或內部系統|⭐（密碼易被攔截）|
|**OAuth 2.0**|第三方授權（Google/Facebook 登入）|⭐⭐⭐⭐⭐（推薦）|
|**JWT（JSON Web Token）**|Web、Mobile API 驗證|⭐⭐⭐⭐（常用）|
|**Session Token**|Web 應用傳統登入|⭐⭐⭐（需儲存 Session）|

---

## **📍 3. API 授權方式詳細解析**

### **✅ 1. API Key（簡單但較不安全）**

API Key 是一組 **唯一的密鑰**，用來識別 API 用戶。

📌 **使用方式**

```http
GET /api/orders 
Authorization: ApiKey abc123xyz
```

📌 **優點**
- 簡單、易於實作 📌 **缺點**
- **容易被攔截，需搭配 HTTPS**
- **無法撤銷單個 API Key**

---

### **✅ 2. Basic Authentication（基本認證，不建議）**

使用 `Base64(username:password)` 進行身份驗證。

📌 **使用方式**

```http
Authorization: Basic dXNlcm5hbWU6cGFzc3dvcmQ=  // Base64(username:password)
```

📌 **優點**
- 實作簡單 

📌 **缺點**
- **需要每次發送用戶名 & 密碼（容易被攔截）**
- **不適用於現代應用（推薦 OAuth 或 JWT）**

---

### **✅ 3. OAuth 2.0（推薦，適用於第三方應用）**

OAuth 2.0 是 **業界標準的授權方式**，適用於**社群登入、第三方 API 授權**。

📌 **流程** 
1. **用戶登入（Google、Facebook）**  
2. **取得 `Authorization Code`**  
3. **交換 `Access Token`**  
4. **使用 `Access Token` 存取 API**

📌 **使用方式**

```http
Authorization: Bearer ya29.a0AfH6SM...
```

📌 **優點**
- **適合第三方 API（如 Google API）**
- **可設定 Token 失效時間**
- **支援不同授權方式（如 `Authorization Code`、`Client Credentials`）**

---

### **✅ 4. JWT（JSON Web Token，推薦）**

JWT 是一種 **無狀態（stateless）** 的授權機制，適合 Web & Mobile API 驗證。

📌 **JWT 結構**

```css
Header.Payload.Signature
```

📌 **使用方式**

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

📌 **優點**
- **無需存儲 Session，適合微服務架構**
- **可攜帶用戶資訊（如 `user_id`）**
- **支持 Token 失效時間（防止長期存取）** 

📌 **缺點**
- Token **無法撤銷**（需設定過期時間）

---

### **✅ 5. Session Token（適用於 Web 登入）**

傳統 Web 登入方式，伺服器會產生 `Session ID`，存放在 `Cookie`。

📌 **使用方式**

```http
Set-Cookie: sessionId=abcdef123456;
```

📌 **優點**
- 適用於 **瀏覽器登入** 
  
📌 **缺點**
- 需要 **Session 存儲（有負擔）**
- 需 **防範 CSRF 攻擊**

---

## **📌 6. API 授權方式比較**

|**方式**|**適用場景**|**安全性**|**是否推薦**|
|---|---|---|---|
|**API Key**|內部 API、簡單授權|⭐⭐⭐|✅（適用內部）|
|**Basic Auth**|測試 API|⭐|❌（不建議）|
|**OAuth 2.0**|第三方登入、授權|⭐⭐⭐⭐⭐|✅✅（推薦）|
|**JWT**|Web & Mobile API|⭐⭐⭐⭐|✅✅（推薦）|
|**Session Token**|Web 登入|⭐⭐⭐|✅（適用 Web）|

---

## **💡 總結**

1. **內部 API** → 使用 `API Key`  
2. **測試 API** → `Basic Auth`（不建議）  
3. **第三方授權（Google/Facebook 登入）** → `OAuth 2.0`（推薦）  
4. **Web / Mobile API 登入** → `JWT`（推薦）  
5. **Web 應用登入（Session 方式）** → `Session Token`

---

🔥 **面試技巧** 如果面試官問：「API 授權方式有哪些？」  
✅ **回答**：「API 授權方式包括 **API Key、Basic Auth、OAuth 2.0、JWT、Session Token**，其中 **OAuth 2.0** 和 **JWT** 是現代應用最常用的方法。」