# Execution Plan: Update Clubs Grid After Post

## Feature Objective

Implement a non-blocking UI update for the Clubs grid. After a user saves a club through the modal form, the form will be replaced by a success message, and the underlying grid on the `Clubs/Index` page will automatically refresh to display the latest data without requiring a full page reload. This will be achieved using htmx for modern, AJAX-driven interactions.

## Data Modeling

No changes are required to the database schema or entities for this feature.

## File Structure

### New Files
- `src/LigaBotonera/Pages/Clubs/_Grid.cshtml`: A new partial view to encapsulate the clubs grid markup, allowing it to be refreshed independently.

### Modified Files
- `src/LigaBotonera/Pages/Clubs/Index.cshtml`: To add htmx attributes for triggering the grid refresh and to render the new `_Grid.cshtml` partial.
- `src/LigaBotonera/Pages/Clubs/Index.cshtml.cs`: To modify the `OnPostSave` handler to return a partial view and to add a new `OnGetGridAsync` handler for refreshing the grid data.
- `src/LigaBotonera/Pages/Clubs/_Form.cshtml`: To add htmx attributes to the form tag, enabling AJAX submission.

## Step-by-Step Implementation

1.  **Isolate the Grid Markup:**
    - Create a new partial view file: `src/LigaBotonera/Pages/Clubs/_Grid.cshtml`.
    - Move the HTML table structure responsible for listing the clubs from `Index.cshtml` into this new `_Grid.cshtml` file.
    - The model for this partial will be an `IEnumerable<Club>`.

2.  **Update the Index Page:**
    - In `Index.cshtml`, replace the extracted grid markup with a `div` container that wraps a call to the new partial.
    - This `div` will be configured with an ID and htmx attributes to listen for the custom event and fetch the updated content.

    ```html
    <div id="clubs-grid" hx-get="/Clubs?handler=Grid" hx-trigger="club-saved from:body" hx-swap="outerHTML">
        <partial name="_Grid" model="Model.Clubs" />
    </div>
    ```

3.  **Create a Dedicated Grid Handler:**
    - In `Index.cshtml.cs`, add a new handler method `OnGetGridAsync` that will be called by htmx to fetch the latest list of clubs.
    - This handler will retrieve the data from the database and return the `_Grid.cshtml` partial view populated with the fresh data.

4.  **Enable AJAX Form Submission:**
    - In `_Form.cshtml`, modify the `<form>` element to include htmx attributes for posting the data via AJAX.
    - The `hx-target` will point to the modal's content area, so the success message replaces the form upon successful submission.

    ```html
    <form method="post" hx-post="/Clubs?handler=Save" hx-target="#modal-content" hx-swap="innerHTML">
        ...
    </form>
    ```

5.  **Modify the Save Handler (`OnPostSave`):**
    - In `Index.cshtml.cs`, refactor the `OnPostSave` method to handle htmx requests.
    - Upon successful validation and data persistence, the method will:
        a. Add an `HX-Trigger` header to the HTTP response. This header will carry the `club-saved` event, signaling the grid on the main page to refresh itself.
        b. Return the `_ModalDialog.cshtml` partial view with a success message. This partial will replace the form inside the modal, confirming the operation to the user.
    - If model validation fails, the handler will return the `_Form.cshtml` partial with the validation errors, which will also be rendered inside the modal.
