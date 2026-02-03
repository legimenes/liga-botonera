
# Feature Objective
Implement a confirmation dialog before deleting a record from the `_Form` partial view. The confirmation will use the existing `_ModalDialog` partial. After a successful deletion, the underlying grid on the `Index` page must be updated.

# Data Modeling
No changes to the database schema or entities are required for this feature.

# File Structure
The following files will be modified:

- **`src/LigaBotonera/Pages/Clubs/_Form.cshtml`**: Modify the `Excluir` (Delete) button to trigger the confirmation modal instead of a direct post.
- **`src/LigaBotonera/Pages/Clubs/Index.cshtml.cs`**: Add handlers to show the confirmation dialog (`OnGetDeleteConfirmation`) and to process the deletion (`OnPostDeleteAsync`). The `OnPostDeleteAsync` handler will be responsible for returning the updated grid and a success message.
- **`src/LigaBotonera/Pages/Shared/ModalDialog/_ModalDialog.cshtml`**: Ensure the "Confirm" button can trigger a POST action to a URL provided via its view model, making it reusable.
- **`src/LigaBotonera/Pages/Clubs/Index.cshtml`**: Ensure it has the necessary elements with `id` attributes for HTMX to swap content, specifically for the grid and the modal.

# Step-by-Step Implementation
1.  **Modify `_Form.cshtml`**: Change the "Excluir" button to an anchor tag `<a>` styled as a button. It will use `hx-get` to call a new handler `OnGetDeleteConfirmation` on the `Index` page, passing the club's `Id`. The `hx-target` will be the modal container.
2.  **Add `OnGetDeleteConfirmation` Handler**: In `Index.cshtml.cs`, create the `OnGetDeleteConfirmation(int id)` handler. This handler will return a `PartialViewResult` for `_ModalDialog.cshtml` populated with a `ModalDialogViewModel` configured for a `Question` type. The view model will include the confirmation message and the URL for the actual delete action (e.g., `/Clubs?handler=Delete&id={id}`).
3.  **Enhance `_ModalDialog.cshtml`**: The "Confirmar" (Confirm) button will be modified to use the URL passed in the `ModalDialogViewModel`. It will have `hx-post` pointing to this URL. The response from this POST (the updated grid and success message) will be swapped into the page.
4.  **Implement `OnPostDeleteAsync` Handler**: In `Index.cshtml.cs`, create the `OnPostDeleteAsync(int id)` handler.
    - It will find and delete the club from the database.
    - It will then re-fetch the list of clubs.
    - It will return a response containing two partial views using HTMX's out-of-band swap mechanism:
        - The updated `_Grid` partial view.
        - The `_ModalDialog` partial view with a success message.
5.  **Update `Index.cshtml`**: Verify that the grid and modal containers have stable `id`s for HTMX to target for swapping.
