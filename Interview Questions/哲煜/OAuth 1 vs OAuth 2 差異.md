#### 📅 **Date**: 2025-03-13

#### 🔖 **Tags**: #Basic #BackEnd #InterviewQuestions

---

OAuth（Open Authorization）是一種**授權協議**，允許第三方應用安全地存取使用者的資源，而**不需要共享密碼**。OAuth 1.0 和 OAuth 2.0 都是為了相同的目標，但 OAuth 2.0 進行了許多**簡化與改進**。

---

## **📍 OAuth 1 vs OAuth 2 主要差異**

|**比較項目**|**OAuth 1.0**|**OAuth 2.0**|
|---|---|---|
|**發佈年份**|2010 年|2012 年|
|**安全機制**|需要 **數位簽章（HMAC-SHA1）**|使用 **SSL/TLS 加密**|
|**身份驗證方式**|**基於密鑰（Key & Secret）**，需要**簽名**|**使用 Token（Access Token）**，簡化身份驗證|
|**存取 Token**|**需要 `oauth_token` 和 `oauth_token_secret`**|**單純 `access_token`（JWT、Bearer Token）**|
|**適用場景**|適用於高安全性需求（如銀行 API）|適用於 Web、Mobile App、微服務|
|**授權流程**|**複雜**，需要多次請求|**簡單**，可以使用不同的授權模式|
|**支援的授權方式**|**單一授權方式**|**多種授權方式（Implicit、Authorization Code、Client Credentials、Password）**|
|**適用設備**|**主要用於 Web 應用**|**適用於 Web、行動裝置、IoT**|

---

## **📍 1. OAuth 1.0 運作方式**

1. OAuth 1.0 需要**數位簽章**來確保請求安全，整個流程比較複雜： 
2. **客戶端請求 `Request Token`**（需要 `consumer_key` & `consumer_secret`）  
3. **使用者授權**（跳轉到提供者授權頁面）  
4. **交換 `Access Token`**（需要 `oauth_verifier`）  
5. **使用 `Access Token` 訪問 API**

📌 **缺點**
- **需要數位簽章（HMAC-SHA1），計算較為複雜**
- **每次請求都要重新簽名**
- **不適合移動裝置（計算量大）**

---

## **📍 2. OAuth 2.0 運作方式**

OAuth 2.0 **簡化了流程**，只需透過 `Access Token` 來存取 API： 
1. **用戶授權**（跳轉到提供者登入頁面）  
2. **交換 `Access Token`**（使用 `Authorization Code`）  
3. **使用 `Access Token` 訪問 API**

📌 **優勢**
- **簡單易用，不需數位簽章**
- **支援多種授權模式（適用 Web、Mobile、Server）**
- **可使用 `JWT（JSON Web Token）` 作為 `Access Token`**

---

## **📍 3. OAuth 2.0 的授權方式**

OAuth 2.0 提供了**四種授權模式**：

|**授權模式**|**適用場景**|**流程說明**|
|---|---|---|
|**Authorization Code（授權碼模式）**|Web 應用、伺服器端|最安全，使用授權碼交換 Token|
|**Implicit（隱式授權模式）**|單頁應用（SPA）|不安全，Token 直接暴露給前端|
|**Client Credentials（用戶端模式）**|機器對機器（M2M API）|用戶不參與，直接授權 API 存取|
|**Password Grant（密碼模式）**|受信任的 App（內部系統）|直接使用帳密取得 Token，不安全|

---

## **📍 4. OAuth 1 vs OAuth 2 實際 API 請求對比**

### **OAuth 1.0**

每次請求都需要**數位簽章（Signature）**：

```http
GET /resource HTTP/1.1
Host: api.example.com
Authorization: OAuth oauth_consumer_key="key",
				oauth_token="token",
				oauth_signature_method="HMAC-SHA1",
				oauth_timestamp="123456789",
				oauth_nonce="random",
				oauth_signature="computed_signature"
```

🔹 **需要手動計算 `oauth_signature`，較為複雜**。

---

### **OAuth 2.0**

OAuth 2.0 使用 **Bearer Token**，不需簽名：

```http
GET /resource HTTP/1.1
Host: api.example.com
Authorization: Bearer your_access_token
```

🔹 **簡單易懂，API 直接透過 Token 驗證身份**。

---

## **💡 總結**

### **OAuth 1 vs OAuth 2 差異重點**

|              | **OAuth 1.0**           | **OAuth 2.0**                 |
| ------------ | ----------------------- | ----------------------------- |
| **安全機制**     | **數位簽章（HMAC-SHA1）**     | **SSL/TLS 加密 + Access Token** |
| **授權方式**     | 單一模式                    | 多種授權模式                        |
| **存取 Token** | 需要 `oauth_token_secret` | 只需 `access_token`             |
| **API 請求**   | 需簽名（Signature）          | 直接使用 `Bearer Token`           |
| **適用場景**     | 高安全需求（銀行、支付）            | Web、Mobile、IoT、API            |

---

🔥 **面試技巧** 如果面試官問：「OAuth 1 和 OAuth 2 的主要差異？」  
✅ **回答**：「OAuth 2 更簡單，不需要簽名，支援多種授權模式，適用於 Web、行動裝置和 API，OAuth 1 則較為複雜，適用於高安全需求的應用。」