param
(
	[Parameter(Mandatory=$true)]
	[string]$File,

	[Parameter(Mandatory=$true)]
	[array]$Tokens,

	[Parameter(Mandatory=$true)]
	[array]$Replacements,

	[Parameter(Mandatory=$false)]
	[array]$TokenPattern = "###TOKEN###"
)

$command = "Command Excuted: " + $MyInvocation.Line
Write-Output $command

if($Tokens.Count -ne $Replacements.Count){
	throw "The elements count does not match between Tokens and Replacements."
}
else{
	if(Test-Path $File){
		$filecontent = Get-Content($File)
		attrib $file -r
		for ($i=0; $i -le $Tokens.Count-1; $i++) {
			$token = $TokenPattern -replace "TOKEN", $Tokens[$i]
			Write-Output "Replacing token $token..."
			$filecontent = $filecontent -replace $token,  $Replacements[$i]
   		}
		Out-File -InputObject $filecontent -FilePath $File -Encoding utf8
		Write-Output "Done."
	}
	else{
		throw "File not found '$File'"
	}
}