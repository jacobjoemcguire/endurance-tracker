variable "resource_group_name" {
  type = string
}

variable "location" {
  type = string
}

variable "project_name" {
  type = string
}

variable "environment" {
  type = string
}

variable "key_vault_uri" {
  type = string
}

variable "sql_connection_string" {
  type      = string
  sensitive = true
}

variable "application_insights_connection" {
  type      = string
  sensitive = true
}

variable "entra_tenant_id" {
  type = string
}

variable "entra_client_id" {
  type = string
}

variable "tags" {
  type    = map(string)
  default = {}
}
