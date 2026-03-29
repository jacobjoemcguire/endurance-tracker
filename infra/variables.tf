variable "project_name" {
  description = "Project name used in resource naming"
  type        = string
  default     = "endurance-tracker"
}

variable "environment" {
  description = "Environment (dev, staging, prod)"
  type        = string
  default     = "dev"
}

variable "location" {
  description = "Azure region"
  type        = string
  default     = "uksouth"
}

variable "sql_admin_login" {
  description = "SQL Server administrator login"
  type        = string
  sensitive   = true
}

variable "sql_admin_password" {
  description = "SQL Server administrator password"
  type        = string
  sensitive   = true
}

variable "entra_tenant_id" {
  description = "Microsoft Entra ID tenant ID"
  type        = string
}

variable "entra_client_id" {
  description = "Microsoft Entra ID application client ID"
  type        = string
}
