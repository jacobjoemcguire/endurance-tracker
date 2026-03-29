# Entra ID App Registration Setup

## Prerequisites
- An Azure subscription with Microsoft Entra ID (Azure AD)
- Azure CLI installed and authenticated (`az login`)

## Step 1: Create the App Registration

```bash
az ad app create \
  --display-name "EnduranceTracker" \
  --sign-in-audience "AzureADMyOrg" \
  --web-redirect-uris "http://localhost:3000" \
  --enable-id-token-issuance true \
  --enable-access-token-issuance true
```

Note the `appId` (Client ID) from the output.

## Step 2: Get Your Tenant ID

```bash
az account show --query tenantId -o tsv
```

## Step 3: Expose an API Scope

```bash
# Set the Application ID URI
az ad app update --id <app-id> --identifier-uris "api://<app-id>"

# Add the access_as_user scope
az ad app update --id <app-id> --set api='{"oauth2PermissionScopes":[{"adminConsentDescription":"Access EnduranceTracker API as the signed-in user","adminConsentDisplayName":"Access as user","id":"'$(uuidgen)'","isEnabled":true,"type":"User","userConsentDescription":"Access EnduranceTracker API on your behalf","userConsentDisplayName":"Access as user","value":"access_as_user"}]}'
```

## Step 4: Grant Admin Consent (Single User)

```bash
# Create a service principal
az ad sp create --id <app-id>

# Grant admin consent
az ad app permission admin-consent --id <app-id>
```

## Step 5: Configure Local Development

### .NET API (`api/EnduranceTracker.Api/appsettings.Development.json`)
```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "<your-tenant-id>",
    "ClientId": "<your-client-id>",
    "Audience": "api://<your-client-id>"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=EnduranceTracker;User Id=sa;Password=LocalDev123!;TrustServerCertificate=True;"
  }
}
```

### Next.js Frontend (`web/.env.local`)
```
NEXT_PUBLIC_AZURE_AD_CLIENT_ID=<your-client-id>
NEXT_PUBLIC_AZURE_AD_TENANT_ID=<your-tenant-id>
NEXT_PUBLIC_API_URL=http://localhost:5000
NEXT_PUBLIC_REDIRECT_URI=http://localhost:3000
```

## Step 6: Add Production Redirect URIs (After Terraform Apply)

```bash
az ad app update --id <app-id> \
  --web-redirect-uris \
    "http://localhost:3000" \
    "https://app-endurance-tracker-web-dev.azurewebsites.net"
```

## Configuration Reference

| Setting | Value | Where Used |
|---------|-------|------------|
| Client ID | `<from step 1>` | API appsettings, Frontend .env, Terraform vars |
| Tenant ID | `<from step 2>` | API appsettings, Frontend .env, Terraform vars |
| Audience | `api://<client-id>` | API appsettings |
| Scope | `api://<client-id>/access_as_user` | Frontend MSAL config |
