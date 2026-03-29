terraform {
  required_version = ">= 1.5.0"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.65"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "main" {
  name     = "rg-${var.project_name}-${var.environment}"
  location = var.location

  tags = local.common_tags
}

locals {
  common_tags = {
    project     = var.project_name
    environment = var.environment
    managed_by  = "terraform"
  }
}

module "monitoring" {
  source = "./modules/monitoring"

  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  project_name        = var.project_name
  environment         = var.environment
  tags                = local.common_tags
}

module "keyvault" {
  source = "./modules/keyvault"

  resource_group_name   = azurerm_resource_group.main.name
  location              = azurerm_resource_group.main.location
  project_name          = var.project_name
  environment           = var.environment
  tenant_id             = var.entra_tenant_id
  api_principal_id      = module.app_service.api_identity_principal_id
  web_principal_id      = module.app_service.web_identity_principal_id
  tags                  = local.common_tags
}

module "sql" {
  source = "./modules/sql"

  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  project_name        = var.project_name
  environment         = var.environment
  sql_admin_login     = var.sql_admin_login
  sql_admin_password  = var.sql_admin_password
  tags                = local.common_tags
}

module "app_service" {
  source = "./modules/app-service"

  resource_group_name              = azurerm_resource_group.main.name
  location                         = azurerm_resource_group.main.location
  project_name                     = var.project_name
  environment                      = var.environment
  key_vault_uri                    = module.keyvault.vault_uri
  sql_connection_string            = module.sql.connection_string
  application_insights_connection  = module.monitoring.application_insights_connection_string
  entra_tenant_id                  = var.entra_tenant_id
  entra_client_id                  = var.entra_client_id
  tags                             = local.common_tags
}
