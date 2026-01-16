# Execution Plan: Display Validation Errors via JSON API

This plan details implementing Suggestion 1. It modifies the system to return a JSON object on validation failure.

## 1. Feature Objective

When a validation error occurs on the **Pages/Clubs/Edit** page (inside the **ModalContainer**), the system will display the **MessageModal** component with the errors.

**NOTE:** This approach provides a modal-only error display. The existing inline validation messages next to form fields will no longer appear, as the server will not be returning updated HTML on failure.

## 2. Data Modeling

No changes are required to the database or entities.

## 3. File Structure

- **`src/LigaBotonera/Pages/Clubs/Edit.cshtml.cs`**: Will be modified to return a `JsonResult` on validation failure.
- **`src/LigaBotonera/wwwroot/js/message-modal.js`**: Will be updated to render a list of messages.
- **`src/LigaBotonera/Pages/Clubs/Edit.cshtml`**: The embedded JavaScript will be modified to handle the new JSON response.

## 4. Step-by-Step Implementation

### Step 1: Update `message-modal.js` to Display a List

1.  Open `src/LigaBotonera/wwwroot/js/message-modal.js`.
2.  Locate the `window.showMessageModal` function.
3.  Replace the line `modalContent.textContent = options.message || '';` with logic to handle an array of messages.
    -   The new logic will first clear `modalContent`. It will then check if `options.messages` is an array. If so, it will create and append a `<ul>` list with each error message in an `<li>`. If not, it will fall back to the old behavior using `options.message`.

### Step 2: Modify Backend (`Edit.cshtml.cs`) to Return JSON

1.  Open `src/LigaBotonera/Pages/Clubs/Edit.cshtml.cs`.
2.  In the `OnPostAsync` method, locate the `if (!validationResult.IsValid)` block.
3.  **Remove** the `TempData.Set(...)` call and the `return Page();` statement from this block.
4.  In their place, add the following logic:
    -   Create a `MessageModalViewModel` instance with `Type = MessageModalType.Error`, `Title = "Atenção!"`, and `Messages` populated from `validationResult.Errors`.
    -   Return a `BadRequestObjectResult` with the view model. This will send a `400 Bad Request` status code with the error data serialized as a JSON payload. For example: `return new BadRequestObjectResult(new MessageModalViewModel(...));`.

### Step 3: Update Frontend (`Edit.cshtml`) to Process JSON

1.  Open `src/LigaBotonera/Pages/Clubs/Edit.cshtml`.
2.  Find the `<script>` section at the end of the file.
3.  In the `submit` event handler for `#editClubForm`, locate the `else` block that handles failed `fetch` responses.
4.  **Replace** the entire content of the `else` block (`response.text().then(...)`) with new logic:
    -   The new code will parse the JSON body of the error response: `response.json().then(modalData => ...)`.
    -   It will then call `window.showMessageModal(modalData)` to display the errors.
    -   The modal content will **not** be updated with new HTML, as none is being provided by the server in this scenario. The user will see the modal and can close it to correct the data in the unchanged form.
