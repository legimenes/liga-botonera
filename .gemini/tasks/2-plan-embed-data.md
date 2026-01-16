# Execution Plan: Display Validation Errors via Embedded HTML Data

This plan details implementing Suggestion 2 (the recommended approach). It modifies the system to embed modal data within the HTML response on validation failure, preserving inline validation messages.

## 1. Feature Objective

When a validation error occurs on the **Pages/Clubs/Edit** page (inside the **ModalContainer**), the system must display the **MessageModal** component with the list of validation errors, while also preserving the existing inline validation messages next to the form fields.

## 2. Data Modeling

No changes are required to the database or entities.

## 3. File Structure

- **`src/LigaBotonera/Pages/Clubs/Edit.cshtml.cs`**: Will be modified to pass modal data to its own view upon validation failure.
- **`src/LigaBotonera/wwwroot/js/message-modal.js`**: Will be updated to render a list of messages.
- **`src/LigaBotonera/Pages/Clubs/Edit.cshtml`**: Will be modified to embed the modal data in a script tag and have the main script use it after the content is refreshed.

## 4. Step-by-Step Implementation

### Step 1: Update `message-modal.js` to Display a List

1.  Open `src/LigaBotonera/wwwroot/js/message-modal.js`.
2.  Locate the `window.showMessageModal` function.
3.  Replace the line `modalContent.textContent = options.message || '';` with logic to handle an array of messages.
    -   The new logic will first clear `modalContent`. It will then check if `options.messages` is an array. If so, it will create and append a `<ul>` list with each error message in an `<li>`. If not, it will fall back to the old behavior using `options.message`.

### Step 2: Modify Backend (`Edit.cshtml.cs`) to Pass Data to View

1.  Open `src/LigaBotonera/Pages/Clubs/Edit.cshtml.cs`.
2.  Add a new public property to the `Edit` PageModel class: `public MessageModalViewModel? ValidationModal { get; set; }`.
3.  In the `OnPostAsync` method, locate the `if (!validationResult.IsValid)` block.
4.  **Remove** the `TempData.Set(...)` call.
5.  In its place, instantiate a `MessageModalViewModel` and assign it to the new `ValidationModal` property. The view model should have `Type = MessageModalType.Error`, `Title = "Atenção!"`, and `Messages` populated from `validationResult.Errors`.
6.  The `return Page();` statement remains unchanged.

### Step 3: Update Frontend (`Edit.cshtml`) to Embed and Process Data

This involves two parts: embedding the data in the view and updating the main script to use it.

1.  **Embed Data in View:**
    -   Open `src/LigaBotonera/Pages/Clubs/Edit.cshtml`.
    -   Add the following Razor code block just **after** the `</form>` tag and **before** the `@section Scripts` block. This will render a script tag containing the modal data only when a validation error occurs.
      ```csharp
      @if (Model.ValidationModal is not null)
      {
          <script id="validation-modal-script">
              window.validationModalData = @Json.Serialize(Model.ValidationModal);
          </script>
      }
      ```

2.  **Process Data in Main Script:**
    -   In the same file, find the `<script>` section at the end (`@section Scripts`).
    -   In the `submit` event handler for `#editClubForm`, locate the `else` block that handles failed `fetch` responses.
    -   Modify the block to look like this:
      ```javascript
      } else {
          response.text().then(html => {
              // Replace the modal content, which now includes the new HTML and the script with our data
              document.getElementById('modal-container-content').innerHTML = html;

              // Check if the script injected our data
              if (window.validationModalData) {
                  // Show the modal and clean up
                  window.showMessageModal(window.validationModalData);
                  delete window.validationModalData;
              }
          });
      }
      ```
    -   This code first replaces the HTML, which also executes the embedded `<script>` tag, setting the `window.validationModalData` variable. It then immediately reads that variable to show the modal and cleans up after itself.
