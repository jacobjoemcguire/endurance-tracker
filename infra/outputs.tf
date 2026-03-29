output "resource_group_name" {
  value = azurerm_resource_group.main.name
}

output "api_url" {
  value = module.app_service.api_url
}

output "web_url" {
  value = module.app_service.web_url
}

output "sql_server_fqdn" {
  value = module.sql.server_fqdn
}

output "key_vault_uri" {
  value = module.keyvault.vault_uri
}

output "application_insights_connection_string" {
  value     = module.monitoring.application_insights_connection_string
  sensitive = true
}
