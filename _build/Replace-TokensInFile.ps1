param
(
	[Parameter(Mandatory=$true)]
	[string]$TokenizedFile,

	[Parameter(Mandatory=$true)]
	[array]$Tokens,

	[Parameter(Mandatory=$true)]
	[array]$Replacements,

	[Parameter(Mandatory=$false)]	
	[string]$TargetConfigFile, #If Defined, the TargetConfigFile will be replaced by the TokenizedFile after replacements occurs.

	[Parameter(Mandatory=$false)]
	[array]$TokenPattern = "###TOKEN###"
)

$command = "Command Excuted: " + $MyInvocation.Line
Write-Output $command

if($Tokens.Count -ne $Replacements.Count){
	throw "The elements count does not match between Tokens and Replacements."
}
else{
	if(Test-Path $TokenizedFile){
		$TokenizedFilecontent =(Get-Content $TokenizedFile)
		attrib $TokenizedFile -r
		for ($i=0; $i -le $Tokens.Count-1; $i++) {
			$token = $TokenPattern -replace "TOKEN", $Tokens[$i]
			
			if($TokenizedFilecontent -match $token){
				Write-Output "Replacing token '$token' --> File content DOES HAVE the toke '$token'. The token will be replaced."
			}
			else {
				
				Write-Output "Replacing token '$token' --> File content DOES NOT HAVE the '$token'. The token WILL NOT BE replaced."
			}
			$TokenizedFilecontent = $TokenizedFilecontent.Replace($token,  $Replacements[$i])

		}
		
		if($TargetConfigFile){
			if((Test-Path $TargetConfigFile) -eq $false){
				Write-Output "Target config file does not exists. It will be created."
			}
			Out-File -InputObject $TokenizedFilecontent -FilePath $TargetConfigFile -Encoding utf8 -Force -ErrorAction Continue
		}
		else{
			Out-File -InputObject $TokenizedFilecontent -FilePath $TokenizedFile -Encoding utf8
		Write-Output "Done."
		}
	}
	else{
		throw "File not found '$TokenizedFile'"
	}
}