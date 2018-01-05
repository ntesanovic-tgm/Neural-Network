﻿// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
let ran = Random()

type NeuralNetwork = 
    {
        top: int list
        network: float array array
        input: float list
        weights: float array array array
        biases: float array array
    }

let createNN nn = 
    let nodes = Array.init nn.top.Length (fun x -> Array.init nn.top.[x] (fun y -> if x = 0 then nn.input.[y] else 1.0))
    let updatedNetwork = { nn with network = nodes }
    updatedNetwork
   
let normalizeInputs nn = 
    let normInputs = nn.input |> List.map (fun x -> ((x-0.0)/(30.0-0.0))*(1.0-0.0)+0.0)
    let updatedNetwork = { nn with input = normInputs; }
    updatedNetwork

let createWeights nn = 
    let newWeights = Array.init (nn.top.Length-1) (fun x -> Array.init nn.top.[x] (fun y -> Array.init nn.top.[x+1] (fun z -> ran.NextDouble())))
    let updatedNetwork = { nn with weights = newWeights }
    updatedNetwork

let createBiases nn =
    let newBiases = Array.init (nn.top.Length-1) (fun x -> Array.init nn.top.[x+1] (fun y -> ran.NextDouble()))
    let updatedNetwork = { nn with biases = newBiases }
    updatedNetwork

let sigmoid (inpu: float) = 1.0 / (1.0 + 2.7 ** (-inpu))
    
let propagate_forward nn = 

    let updateThese = nn.network

    // todo: propagate_forward without mutable
    for x in 0..(nn.weights.Length-1) do
        for y in 0..(nn.weights.[x].Length-1) do
            for z in 0..(nn.weights.[x].[y].Length-1) do
                updateThese.[x+1].[z] <- nn.network.[x+1].[z] + (nn.weights.[x].[y].[z] * nn.network.[x].[y])

    for x in 0..(nn.network.Length-2) do
        for y in 0..(nn.network.[x+1].Length-1) do
            updateThese.[x+1].[y] <- nn.network.[x+1].[y] * nn.biases.[x].[y]

    for x in 0..(nn.network.Length-2) do
        for y in 0..(nn.network.[x+1].Length-1) do
            updateThese.[x+1].[y] <- sigmoid(nn.network .[x+1].[y])

    let updatedNetwork = { nn with network = updateThese }
    updatedNetwork

[<EntryPoint>]
let main argv = 

    let neuralnetwork = 
        {
            top     = [2; 3; 1]
            network = [||]
            input   = [7.0; 15.0]
            weights = [||]
            biases  = [||]
        }

    let updated nn = 
        nn
        |> normalizeInputs
        |> createNN
        |> createWeights
        |> createBiases
        |> propagate_forward
        

    printfn "%A" (updated neuralnetwork)

    Console.ReadLine() |> ignore

    0 // return an integer exit code
