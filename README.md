# Blazor Server PayPal Integration Demo

This project demonstrates how to integrate PayPal payment processing into a Blazor Server application using server-side .NET code and component-scoped JavaScript.


<a href="https://buymeacoffee.com/alex_m" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" height="41" width="174"></a>

## Project Overview

This is a Blazor Server application that enables users to make payments through PayPal. The application leverages Blazor Server's server-side execution model to securely handle PayPal API calls while providing an interactive user experience through SignalR.

## Features

- Complete PayPal checkout flow integration
- Secure server-side payment processing
- PayPal credentials safely stored on the server (never exposed to the browser)
- Interactive payment buttons with **Blazor Page Scripts** (`.razor.js` files)
- Component-scoped JavaScript using ES6 modules
- Comprehensive payment lifecycle management
- Responsive UI built with Bootstrap 5
- Real-time updates via SignalR
- Sandbox and production environment support

## Setup Instructions

### Prerequisites

- .NET 8.0 SDK or later
- A PayPal Developer account

### Configuration

Edit `appsettings.json`:

```json
{
  "PayPal": {
    "ClientId": "<YOUR-PAYPAL-CLIENT-ID>",
    "ClientSecret": "<YOUR-PAYPAL-CLIENT-SECRET>",
    "UseSandbox": "true"
  }
}
```

**Note:** The `ClientSecret` is securely stored on the server and never sent to the browser, ensuring your PayPal credentials remain protected.

### Run the Application

```bash
dotnet run
```

The application will start and the PayPal checkout form will be displayed on the home page.

## How It Works

### Architecture

This application uses Blazor Server's hybrid architecture that combines server-side .NET execution with client-side interactivity:

1. **Server-Side Processing**: All PayPal API calls are executed on the server using the official PayPal .NET SDK
2. **Client-Side Interaction**: PayPal buttons are rendered in the browser using JavaScript
3. **Real-Time Communication**: SignalR maintains a persistent connection between browser and server for instant updates

### PayPal Checkout Flow

1. **Initialize**: User enters payment amount and description, then clicks "Initialize PayPal"
2. **Load PayPal SDK**: JavaScript loads the PayPal SDK and renders the checkout buttons
3. **Create Order**: When user clicks the PayPal button, the server creates an order using PayPal's API
4. **User Authentication**: User is redirected to PayPal to log in and approve the payment
5. **Capture Payment**: After approval, the server captures the payment and finalizes the transaction
6. **Confirmation**: User sees a success message with the order ID

### Technical Components

**Server-Side (.NET)**
- `Service/PayPalService.cs` - Handles all PayPal API interactions
  - Creates payment orders
  - Captures approved payments
  - Manages PayPal SDK client configuration
- Uses `PayPalCheckoutSdk` NuGet package for secure API communication
- Credentials and business logic remain on the server

**Client-Side (JavaScript)**
- `Pages/PayPalDemo.razor.js` - Blazor Page Script for PayPal integration
  - Loads the PayPal JavaScript SDK dynamically
  - Renders PayPal checkout buttons
  - Communicates with .NET server methods via JavaScript Interop
- Uses ES6 module imports for clean, component-scoped code

**UI Component**
- `Pages/PayPalDemo.razor` - Main payment interface
  - Collects payment information from user
  - Manages component lifecycle and JavaScript module loading
  - Implements callback methods:
    - `CreateOrderAsync` - Server method called from JavaScript to create orders
    - `OnPaymentApproved` - Handles successful payment completion
    - `OnPaymentCancelled` - Handles cancelled transactions
    - `OnPaymentError` - Handles error scenarios

## PayPal Developer Account

To obtain your Client ID and Client Secret, you need to create a PayPal Developer account at [developer.paypal.com](https://developer.paypal.com) and set up an application.

For testing purposes, it's recommended to use PayPal's Sandbox environment, which allows you to simulate transactions without using real money.

## Security Considerations

- The Client Secret is never exposed to the browser
- All PayPal API calls are executed server-side
- HTTPS is used for all communications
- Blazor Server's SignalR connection is secure by default

## License

[MIT License](../LICENSE.txt)

## Acknowledgements

- [PayPal Developer Documentation](https://developer.paypal.com/docs/checkout/)
- [Blazor Documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/)

# Support the Project

If you find this project helpful, consider supporting its development! Your support helps maintain and improve this learning demo.

## Buy Me a Coffee

Enjoy using the demo? You can show your appreciation by buying me a coffee:

[<i class="bi bi-cup-hot-fill"></i> Buy Me a Coffee](https://coff.ee/Alex_M)

## PayPal

Alternatively, you can contribute via PayPal:

[<i class="bi bi-paypal"></i> PayPal.me](https://paypal.me/MayerAlexAndDer)

Every contribution, no matter how small, makes a difference and is greatly appreciated! Thx!
