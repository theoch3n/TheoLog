#### 📅 **Date**: 2025-02-20

#### 🔖 **Tags**: `#Vue` `#Bootstrap` `#BugFix`

---

## **🛠️ 問題描述**

在 `productList.vue` 點擊「立即結帳」按鈕後，透過 **Pinia** 成功傳送商品資訊到 `testPayment.vue`，但頁面切換後仍然出現 **灰色遮罩 (`modal`)**，且需要手動刷新頁面才能移除。

**問題影響：**
- **頁面無法滾動**
- **使用者體驗不佳**
- **Bootstrap 的 `modal` 沒有正確清除**

---

## **🔍 原因分析**

1. **Bootstrap `modal` 沒有被 Vue Router 變更時自動清除**
    - Bootstrap 的 `modal` 會自動在 `body` 添加 `overflow: hidden`，但 Vue Router 切換後，這些樣式 **沒有自動還原**，導致 `testPayment.vue` 仍然顯示遮罩。
2. **`body` 樣式 (`overflow: hidden`) 沒有被正確恢復**
    - `ProductList.vue` 內的 `modal` 在關閉時，沒有正確釋放 `body` 的 CSS 樣式。

---

## **✅ 解決方案**

在 **`testPayment.vue`** 添加以下修正：
1. **`onMounted()` 時移除 `modal-backdrop`，確保 `body` 樣式恢復**
2. **`onUnmounted()` 確保 `body` 不會再被 `modal` 鎖定**
3. **強制清除所有 Bootstrap `modal-backdrop` 避免殘留**

### **📌 具體修改內容**

#### **1. 修正 `onMounted()`**

```JavaScript
// 渲染頁面後清除 body 樣式
onMounted(() => {
    document.body.style.overflow = 'auto';
    document.body.style.position = 'static';

    // 確保 Bootstrap Modal 的灰色背景 (`modal-backdrop`) 被移除
    const backdrops = document.querySelectorAll('.modal-backdrop');
    backdrops.forEach(backdrop => backdrop.remove());
});
```

#### **2. 修正 `onUnmounted()`**

```JavaScript
// 當組件卸載時，也確保 body 樣式恢復
onUnmounted(() => {
    document.body.style.overflow = 'auto';
    document.body.style.position = 'static';
    isDialogVisible.value = false;
});
```

---

## **🎯 修正結果**
- **進入 `testPayment.vue` 時，自動清除 `modal-backdrop`**  
- **恢復 `body` 樣式，確保可正常滾動**  
- **Vue Router 切換後不會再出現灰色遮罩**  
- **所有 `modal` 在離開 `ProductList.vue` 後被正確釋放**

---

## **🛠️ 未來改進**
- **最佳化 Bootstrap `modal` 的管理**：可以考慮 **在 Vue Router `beforeRouteLeave` 鉤子中手動清理 `modal`**，以防止類似問題發生。
- **改用 Vue 的 `v-dialog` 取代 Bootstrap `modal`**：Vue 的 `v-dialog` 可以自動管理開關狀態，避免 `modal-backdrop` 殘留。