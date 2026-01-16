# Execution Plan: Fix Index page error after ModalContainer POST

## Feature Objective

Fix a `NullReferenceException` on the Clubs index page (`Pages/Clubs/Index`) that occurs after saving changes from the "Edit Club" modal. The error is caused by the modal's form submitting a POST request to the wrong URL (`/Clubs/Index` instead of `/Clubs/Edit`). This incorrect routing results in the Index page being re-rendered without its necessary data (`Model.Clubs`), causing the exception.

## Data Modeling

No changes are required to the database or entities.

## File Structure

- **`src/LigaBotonera/Pages/Clubs/Edit.cshtml`**: Will be modified to correct the form's POST destination and fix javascript errors.
- **`src/LigaBotonera/Pages/Clubs/Index.cshtml.cs`**: Will be modified for increased robustness against null-reference errors.

## Step-by-Step Implementation

1.  **Correct Form Action in `Edit.cshtml`**:
    -   In `src/LigaBotonera/Pages/Clubs/Edit.cshtml`, locate the `<form>` tag.
    -   Add the `asp-page="Edit"` attribute to the tag. This will ensure the form's `action` attribute is correctly generated to point to the `/Clubs/Edit` endpoint, so the POST request is handled by `EditModel.OnPostAsync`.

2.  **Fix JavaScript in `Edit.cshtml`**:
    -   Within the same file (`src/LigaBotonera/Pages/Clubs/Edit.cshtml`), find the `<script>` block at the end.
    -   Inside the `submit` event listener for `#editClubForm`:
        -   In the success path of the `fetch` call (`if (response.ok)`), change the function call from `closeGenericModal()` to `closeModalContainer()` to correctly close the modal.
        -   In the error path (`else`), change the element ID from `generic-modal-content` to `modal-container-content` to ensure validation errors are displayed within the correct modal panel.

3.  **Improve Robustness in `Index.cshtml.cs`**:
    -   In `src/LigaBotonera/Pages/Clubs/Index.cshtml.cs`, find the `Clubs` property declaration.
    -   Change the initialization from `public IList<Club> Clubs { get; set; } = default!;` to `public IList<Club> Clubs { get; set; } = [];`. This defensive programming step ensures that `Model.Clubs` is always a non-null collection, preventing the view from crashing and instead rendering an empty table if the data is not loaded for any reason.
