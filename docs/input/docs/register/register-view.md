Title: Register Views
Order: 10
---

Register the views:
```csharp
            SextantHelper.RegisterView<HomeView,HomeViewModel>();
            SextantHelper.RegisterView<FirstModalView,FirstModalViewModel>();
            SextantHelper.RegisterView<SecondModalView, SecondModalViewModel>();
            SextantHelper.RegisterView<RedView, RedViewModel>();
```



Register Navigation views if you need some special configuration on the Navigation, like different colors:
```csharp
            SextantHelper.RegisterNavigation<BlueNavigationView, SecondModalViewModel>();
```