
$dbpath = $PSScriptRoot + "\..\hum.db"
$ddlpath = "'" + $PSScriptRoot + "\..\config\ddl.sql'"
$dmlpath = "'" + $PSScriptRoot + "\..\config\dml.sql'"

if (Test-Path $dbpath) {
    Remove-Item $dbpath
}

$ddlcmd = "sqlite3 " + $dbpath + " "".read " + $ddlpath + """"
Invoke-Expression $ddlcmd

$dmlcmd = "sqlite3 " + $dbpath + " "".read " + $dmlpath + """"
Invoke-Expression $dmlcmd
