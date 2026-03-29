output "api_url" {
  value = "https://${azurerm_linux_web_app.api.default_hostname}"
}

output "web_url" {
  value = "https://${azurerm_linux_web_app.web.default_hostname}"
}

output "api_identity_principal_id" {
  value = azurerm_linux_web_app.api.identity[0].principal_id
}

output "web_identity_principal_id" {
  value = azurerm_linux_web_app.web.identity[0].principal_id
}
