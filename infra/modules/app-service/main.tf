resource "azurerm_service_plan" "main" {
  name                = "asp-${var.project_name}-${var.environment}"
  resource_group_name = var.resource_group_name
  location            = var.location
  os_type             = "Linux"
  sku_name            = "F1"

  tags = var.tags
}

resource "azurerm_linux_web_app" "api" {
  name                = "app-${var.project_name}-api-${var.environment}"
  resource_group_name = var.resource_group_name
  location            = var.location
  service_plan_id     = azurerm_service_plan.main.id

  https_only = true

  identity {
    type = "SystemAssigned"
  }

  site_config {
    application_stack {
      dotnet_version = "8.0"
    }

    always_on = false # Not available on F1
  }

  app_settings = {
    "KeyVault__Uri"                             = var.key_vault_uri
    "AzureAd__TenantId"                         = var.entra_tenant_id
    "AzureAd__ClientId"                         = var.entra_client_id
    "APPLICATIONINSIGHTS_CONNECTION_STRING"      = var.application_insights_connection
  }

  connection_string {
    name  = "DefaultConnection"
    type  = "SQLAzure"
    value = var.sql_connection_string
  }

  tags = var.tags
}

resource "azurerm_linux_web_app" "web" {
  name                = "app-${var.project_name}-web-${var.environment}"
  resource_group_name = var.resource_group_name
  location            = var.location
  service_plan_id     = azurerm_service_plan.main.id

  https_only = true

  identity {
    type = "SystemAssigned"
  }

  site_config {
    application_stack {
      node_version = "20-lts"
    }

    always_on = false
  }

  app_settings = {
    "NEXT_PUBLIC_API_URL"              = "https://app-${var.project_name}-api-${var.environment}.azurewebsites.net"
    "NEXT_PUBLIC_AZURE_AD_TENANT_ID"   = var.entra_tenant_id
    "NEXT_PUBLIC_AZURE_AD_CLIENT_ID"   = var.entra_client_id
  }

  tags = var.tags
}
