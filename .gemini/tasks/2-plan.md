# Execution Plan: Display Validation Errors in ModalContainer

## 1. Feature Objective

When a validation error occurs on the **Pages/Clubs/Edit** page while it is rendered inside the **ModalContainer**, the system must display the **MessageModal** component containing the list of validation errors. This should be done while preserving the existing inline validation messages that appear next to the form fields.

## 2. Data Modeling

No changes are required to the database or entities.

## 3. File Structure

- **`src/LigaBotonera/Pages/Clubs/Edit.cshtml.cs`**: Will be modified to send validation error data in a custom HTTP header instead of using `TempData`.
- **`src/LigaBotonera/wwwroot/js/message-modal.js`**: Will be updated to allow rendering a list of messages instead of just a single string.
- **`src/LigaBotonera/Pages/Clubs/Edit.cshtml`**: The embedded JavaScript will be modified to read the custom HTTP header and trigger the `MessageModal`.

## 4. Step-by-Step Implementation

### Step 1: Modify Backend (`Edit.cshtml.cs`) to Send Errors in Header

The current implementation uses `TempData`, which is not suitable for an AJAX context. I will change this to communicate errors via an HTTP header.

1.  Open `src/LigaBotonera/Pages/Clubs/Edit.cshtml.cs`.
2.  Add `using System.Text.Json;` at the top.
3.  In the `OnPostAsync` method, locate the `if (!validationResult.IsValid)` block.
4.  **Remove** the line: `TempData.Set("MessageModalNotify", ...);`.
5.  In its place, add the following logic:
    -   Create a `MessageModalViewModel` instance. The `Type` should be `MessageModalType.Error`, the `Title` should be "Atenção!", and the `Messages` property should be populated with the list of error messages from `validationResult.Errors`.
    -   Serialize this view model to a JSON string.
    -   Add the JSON string to the response headers under a custom key, like `X-Message-Modal-Data`.
    -   To ensure the browser's JavaScript can access this custom header, add the `Access-Control-Expose-Headers` header.

### Step 2: Update `message-modal.js` to Display a List of Messages

The `showMessageModal` function needs to be able to render a list of errors, not just a single text line.

1.  Open `src/LigaBotonera/wwwroot/js/message-modal.js`.
2.  Locate the `window.showMessageModal` function.
3.  Modify the line `modalContent.textContent = options.message || '';` to handle both a single message (`options.message`) and a list of messages (`options.messages`).
4.  The new logic will first clear `modalContent`. Then, it will check if `options.messages` is an array. If so, it will create a `<ul>` element and populate it with `<li>` elements for each message. If `options.messages` is not an array, it will fall back to using `options.message` as a single text content.

### Step 3: Update Frontend (`Edit.cshtml`) to Process Header

The JavaScript in the Edit page needs to read the header from the failed response and use it to show the modal.

1.  Open `src/LigaBotonera/Pages/Clubs/Edit.cshtml`.
2.  Find the `<script>` section at the end of the file.
3.  In the `submit` event handler for `#editClubForm`, locate the `else` block within the `fetch` promise chain (which handles failed responses).
4.  Inside this block, add logic to:
    -   Check for the existence of the `X-Message-Modal-Data` header in the `response`.
    -   If it exists, parse the JSON content from the header.
    -   Call `window.showMessageModal()` with the parsed data.
    -   Ensure the existing code that updates the modal's HTML content (`modal-container-content`) is preserved, as it is responsible for displaying the inline validation messages.
