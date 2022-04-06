[CmdletBinding()]
Param(
 [Parameter(Mandatory=$True,Position=1)]
 [string]$testRunner,
 [Parameter(Mandatory=$True,Position=2)]
 [string]$testLibrary,
 [Parameter(Mandatory=$True,Position=3)]
 [string[]]$traits,
 [Parameter(Mandatory=$True,Position=4)]
 [string]$outputDir)
 

$error.Clear()

workflow pbatch { 
    param([string] $testrunner, [string] $testLibrary, [string[]] $traits, [string] $outputDir) 

    Write-Output $testrunner
    
    $results = @{}

    foreach -parallel ($trait in $traits) {
        $outData = InlineScript {
            . $Using:testrunner $Using:testLibrary -xml "$Using:outputDir\TEST-$Using:trait-Result.xml" -parallel all -trait $Using:trait 
        } -AppendOutput $true -MergeErrorToOutput $false
     
        Write-output "Trait $trait execution:" $outData "" "" 
     }
}

pbatch  -testrunner $testrunner -testlibrary $testLibrary -traits $traits -outputDir $outputDir

if($error.Count -gt 0){
    Write-Output "Test failed summary:"
    foreach($e in $error){

        Write-Output $e.Exception
    }
    Write-Output "" "Check execution result for details."
}
exit $error.Count 

