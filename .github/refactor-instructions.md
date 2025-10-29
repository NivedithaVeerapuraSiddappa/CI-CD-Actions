# Refactor Instructions: Applying SOLID Principles to the Minimal eCommerce Website

This guide provides step-by-step instructions to refactor the current codebase using SOLID principles for better maintainability and extensibility.

## S — Single Responsibility Principle (SRP)
- **Product Model:** Ensure `Product` only contains product-related properties.
- **Controllers:** Move business logic (e.g., product retrieval) out of controllers into dedicated service classes.

## O — Open/Closed Principle (OCP)
- Design classes so they can be extended without modifying existing code.
- Example: Use interfaces for product services so new product types or retrieval methods can be added without changing the controller.

## L — Liskov Substitution Principle (LSP)
- Use base interfaces or abstract classes for services so derived classes can be substituted without breaking functionality.
- Example: If you add a `DiscountedProductService`, it should be usable wherever a `IProductService` is expected.

## I — Interface Segregation Principle (ISP)
- Split large interfaces into smaller, more specific ones.
- Example: If product services grow, separate interfaces for read, write, and update operations.

## D — Dependency Inversion Principle (DIP)
- Depend on abstractions, not concrete implementations.
- Example: Inject `IProductService` into controllers via constructor injection.

---

## Example Refactor Steps
1. **Create a `IProductService` interface in `Models` or a new `Services` folder.**
2. **Implement a `ProductService` class that handles product retrieval.**
3. **Refactor `HomeController` to use `IProductService` via constructor injection.**
4. **Register `ProductService` in `Program.cs` for dependency injection.**
5. **If needed, split interfaces and add new service implementations following SOLID.**

---

## Benefits
- Easier to test and maintain
- Supports future features (e.g., discounts, categories)
- Promotes clean architecture

Follow these steps to refactor your project and ensure it adheres to SOLID principles.