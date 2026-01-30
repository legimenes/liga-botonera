# Fix overlap behavior between _ModalContainer and _ModalDialog

On the Razor page `Clubs/Index`, when we click the **Create Club** button—which is actually an `<a>` element—the `_ModalContainer` partial view is opened on the screen with the content of the `_Form` partial view. When we click the **Save** button in the `_Form` partial view, the `OnPostSave` method is called.

When execution reaches the condition in the `if` statement below, the `_ModalDialog` partial view is opened to display validation error messages.

```cs
if (!validationResult.IsValid)
```

However, at the moment `_ModalDialog` is opened, `_ModalContainer` disappears. This behavior is incorrect: `_ModalContainer` should remain open underneath `_ModalDialog`.

Analyze the code to apply this fix. If possible, avoid using `Response.Headers` in the solution.