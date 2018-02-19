$scriptpath = split-path -parent $MyInvocation.MyCommand.Definition
$venvpath = $scriptpath + '\..\venv'
$apppath = $scriptpath + '\..\app.py'

$runapp = $venvpath + '\Scripts\python ' + $apppath
Invoke-Expression $runapp
