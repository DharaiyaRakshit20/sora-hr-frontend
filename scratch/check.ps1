$c = Get-Content -Raw -Encoding UTF8 'd:\sora-hr-frontend\super-admin\index.html'
Write-Host "Total length: " $c.Length
Write-Host "Contains editAdminModal: " $c.Contains("editAdminModal")
Write-Host "Contains openEditAdminPage: " $c.Contains("openEditAdminPage")
Write-Host "Contains wizardDept: " $c.Contains("wizardDept")
Write-Host "Contains Department Master: " $c.Contains("Department Master")
Write-Host "Contains openRoleModal: " $c.Contains("openRoleModal")
Write-Host "Contains deleteAdmin: " $c.Contains("deleteAdmin")
Write-Host "Contains toggleAdminStatus: " $c.Contains("toggleAdminStatus")
