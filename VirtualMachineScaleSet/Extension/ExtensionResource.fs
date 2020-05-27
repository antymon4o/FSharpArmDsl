module VirtualMachineScaleSetExtensionResource

open VirtualMachineScaleSetResource

type Extension = {
    apiVersion: string
    ``type``: string
    name: string
    properties: VirtualMachineScaleSetExtensionProperties 
}
