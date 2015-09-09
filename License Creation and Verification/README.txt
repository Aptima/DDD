About the License Creation and Verification Folder:

The TestInstallerDLL project is a key verifier.  Its input is a license key.  It populates labels based on the validity of the key.  It also writes to the registry (which is required for the DDD to run).  The reg destination is LOCALUSERS\SOFTWARE\Aptima\Asim\DDD.

The TestGUIForAptimaLicenseGenerator is a key generator.  You fill in required fields, and it spits out a registration key.  Pretty straightforward.




Thanks for reading!