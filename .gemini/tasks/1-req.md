# Fix Index page error after ModalContainer POST
 
On the **Index** page of **Pages/Clubs**, when the ModalContainer component is rendering the **Pages/Clubs/Edit** page and the **Save** button is clicked, a POST request is sent to the server. After this POST, the Index page is loaded again; however, at this point it throws the error System.NullReferenceException: "Object reference not set to an instance of an object." because in **@foreach (var item in Model.Clubs)**, Model.Clubs is null.

Fix this error.