# Umbraco MVC Bundling

MVC Bundling and Minification can help improve the load time and performance of a site. The majority of [popular browsers](http://www.browserscope.org/?category=network) only allow six parallel requests. This means the initial page load of a site can only request at six files at a time. This can be a problem for sites that require more than six requests and can leave users waiting unnecessarily. The problem can be easily be solved by using bundling and minification. 

Bundling works by compressing the selected css or javascript files into a single newly created file. This will reducing the number of requests.

Minification works by reducing the file size by removing unnecessary characters, white spaces and comments without affecting the styles or functionality. The results of the reduction will be placed inside a newly created min file.

In this guide we will show you how you can utilise these techniques in you own Umbraco installation. 

## Getting started

You must you have installed [“Microsoft ASP.NET Web Optimization”](https://www.nuget.org/packages/Microsoft.AspNet.Web.Optimization/) in your solution to enable mvc bundling and minification. You can install it via nuget or you can run the following command in your package manager console:

```
Install-Package Microsoft.AspNet.Web.Optimization
```

##Register Bundles

First you will need to register your bundles in App_Start/BundleConfig.cs in the method Registerbundles. If your solution does not contain a BundleConfig.cs then you can create one. Once you have register a bundle it will automatically be minified as well.

Here is an example of the BundleConfig.cs

```C# 
public class BundleConfig
{
    // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
    public static void RegisterBundles(BundleCollection bundles)
    {
        bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                    "~/Scripts/jquery-{version}.js"));

        bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/jquery.validate*"));

        // Use the development version of Modernizr to develop with and learn from. Then, when you're
        // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
        bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"));

        bundles.Add(new ScriptBundle("~/bundles/umbraco").Include(
                    "~/Scripts/BundleTest.js"));

        bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                  "~/Scripts/bootstrap.js",
                  "~/Scripts/respond.js"));

        bundles.Add(new StyleBundle("~/Content/css").Include(
                  "~/Content/bootstrap.css",
                  "~/Content/site.css"));
    }
}
```

Here is an example of adding a CSS bundle:
```C# 
bundles.Add(new StyleBundle("~/Content/css").Include(
          "~/Content/bootstrap.css",
          "~/Content/site.css"));
```

Here is an example of adding a Javascript bundle:
```C# 
bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
          "~/Scripts/bootstrap.js",
          "~/Scripts/respond.js"));
```

NOTE: 

* The bundle locations must not exist in the solution.
* The bundle will not override existing minified files.

##Registering Umbraco Events

Once you have set up your bundles you will need Umbraco to register the bundleconfig.cs. The easiest way to do that is to create a application event handler that hooks onto the Umbraco start event. 

For this example we will be creating custom application event handler inherits from Umbraco.Core.IApplicationEventHandler.

```C# 
public class CustomApplicationEventHandler : IApplicationEventHandler
{
    public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
    {
    }

    public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
    {

    }

    public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
    {
        BundleConfig.RegisterBundles(BundleTable.Bundles);
    }
}
```

You will see that the OnApplicationStarting method has been updated to register the bundles on Umbraco start up.

##Updating Umbraco Reserve Paths

You will now need to update the app key “umbracoReservedPaths” located in the web.config. You will need to include the bundle paths you have register in App_Start/BundleConfig.cs. This will prevent Umbraco from intercepting these request.

Here is an example of app key:
```XML 
<add key="umbracoReservedPaths" value="~/umbraco,~/install/,~bundles" />
```

##Bundling and Minification Usage

Finally you can add your bundles to your views.

Here is an example of adding a CSS bundle:
```Razor 
@Styles.Render("~/content/umbraco")
```

Here is an example of adding a Javascript bundle:
```Razor
@Scripts.Render("~/bundles/umbraco")
```

##Testing Locally

Locally your bundles will not be enabled on default so there are two ways to active the bundles and minification.

In web.config:
You can set the debug to false.
```XML 
<compilation defaultLanguage="c#" debug="false" batch="false" targetFramework="4.5">
```

In BundlerConfig:
You can enable bundling in the code.
```C# 
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            BundleTable.EnableOptimizations = true;
        }
```

##More information:

Mvc bundling and minification:
https://www.asp.net/mvc/overview/performance/bundling-and-minification

Registering Umbraco application events: 
https://our.umbraco.org/documentation/reference/events/application-startup

