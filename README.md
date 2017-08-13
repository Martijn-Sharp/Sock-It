# Sock It! #

An effort to make it easy to map websockets to controllers with Microsoft.AspNetCore.MVC

### How to get the project running ###

* Install Visual Studio 2017

### Tests ###

* You can either use the sample projects
or
* Check out the test libraries

### Want to contribute? ###

* Drop me a message

### Ideals ###

* Use the MVC pattern for websockets
* Get websockets working with minimal effort
* Incorporate into the MVC library as much as possible

### Stuff that you could contribute with: ###

* 100% unit test coverage
* Encapsulate a websocket message with metadata (much like HTTP requests) + client side solution
* Get modelbinding validation working so we can use ModelState in the controller
* Get filters working for each request so we can do stuff like authentication (perhaps also link users to the WebSocketContext)

### Stuff that I wish were different/better ###

* I couldn't reuse many solutions made in Microsoft.AspNetCore.MVC.Core because they relied too heavily on the HttpContext, it would be great if someone can think of a workaround

### How does it work? ###

It works very similar to how you'd use regular Controllers. When making a new controller for a websocket connection you inherit from the `WebSocketController` class, like this:
```
public class MyController : WebSocketController
{
	// Here go your awesome methods
}
```

Unlike the regular MVC Controller route handling, the websocket routehandling expects an implicit route, which means there's no default `Index` method which will get called
> Perhaps something to think about, aye?

When making a method in the WebSocketController you can either return an IWebSocketActionResult or ... any object you'd like to return.
```
public IWebSocketActionResult Index()
{
    return View("Index");
}
```

The `View` method will use the Razor engine, if you want to return JSON or XML that's fine too, I made that for you!
```
public IWebSocketActionResult Index()
{
    return Json("nice");
}
```

or
```
public object Index()
{
    return "nice";
}
```

What if you want to supply parameters? No problem!
```
public IWebSocketActionResult Index(YourAwesomeModel model)
{
    return View("Index", model);
}
```

Want do to something query parameter-esque? Okay...
```
public IWebSocketActionResult Index(string query, YourAwesomeModel model)
{
    return View("Index", model);
}
```

That's all there is for now, bye