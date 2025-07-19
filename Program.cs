//Name: Aniruddha Roy
//Date: April 11th 2025
//Project 3

using System;
using System.IO;
using SkiaSharp;

/// <summary>
/// Represents LFSR that generates pseudorandom bits.
/// </summary>
public class LFSR
{
    private string state;
    private int tap;

    /// <summary>
    /// Initializes a new instance of the <see cref="LFSR"/> class with the specified seed and tap position.
    /// </summary>
    public LFSR(string seed, int tap)
    {
        state = seed;
        this.tap = tap;
    }

    /// <summary>
    /// Generates an 8‑bit value by performing 8 consecutive LFSR steps.
    /// </summary>
    public int Step()
    {
        int left = state[0] - '0';
        int tapBit = state[tap] - '0';
        int newBit = left ^ tapBit;

        state = state.Substring(1) + newBit;
        return newBit;
    }

    /// <summary>
    /// Gets the current state of the LFSR.
    /// </summary>
    public byte GetByte()
    {
        int value = 0;
        for (int i = 0; i < 8; i++)
        {
            value = (value << 1) | Step();
        }
        return (byte)value;
    }

    public string GetState() => state;
}

/// <summary>
/// The main program class that implements various operations of a stream cipher using an LFSR.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            ShowHelp();
            return;
        }

        string option = args[0].ToLower();

        try
        {
            switch (option)
            {
                case "cipher":
                    if (args.Length != 3)
                    {
                        Console.WriteLine("Usage: dotnet run Cipher <seed> <tap>");
                        return;
                    }
                    RunCipher(args[1], int.Parse(args[2]));
                    break;

                case "generatekeystream":
                    if (args.Length != 4)
                    {
                        Console.WriteLine("Usage: dotnet run GenerateKeystream <seed> <tap> <steps>");
                        return;
                    }
                    RunGenerateKeystream(args[1], int.Parse(args[2]), int.Parse(args[3]));
                    break;

                case "encrypt":
                    // Text encryption using keystream XOR.
                    if (args.Length != 2)
                    {
                        Console.WriteLine("Usage: dotnet run Encrypt <plaintext>");
                        return;
                    }
                    RunEncrypt(args[1]);
                    break;

                case "decrypt":
                    // Text decryption using keystream XOR.
                    if (args.Length != 2)
                    {
                        Console.WriteLine("Usage: dotnet run Decrypt <ciphertext>");
                        return;
                    }
                    RunDecrypt(args[1]);
                    break;

                case "triplebits":
                    if (args.Length != 5)
                    {
                        Console.WriteLine("Usage: dotnet run TripleBits <seed> <tap> <step> <iteration>");
                        return;
                    }
                    RunTripleBits(args[1], int.Parse(args[2]), int.Parse(args[3]), int.Parse(args[4]));
                    break;

                case "encryptimage":
                    if (args.Length != 4)
                    {
                        Console.WriteLine("Usage: dotnet run EncryptImage <imagefile> <seed> <tap>");
                        return;
                    }
                    RunEncryptImageUsingRandom(args[1], args[2], int.Parse(args[3]));
                    break;

                case "decryptimage":
                    if (args.Length != 4)
                    {
                        Console.WriteLine("Usage: dotnet run DecryptImage <imagefile> <seed> <tap>");
                        return;
                    }
                    RunDecryptImageUsingRandom(args[1], args[2], int.Parse(args[3]));
                    break;

                default:
                    Console.WriteLine($"Unknown option: {option}");
                    ShowHelp();
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Displays the help message with available commands and usage.
    /// </summary>
    static void ShowHelp()
    {
        Console.WriteLine("Stream Cipher with LFSR - Help");
        Console.WriteLine("------------------------------");
        Console.WriteLine("Usage: dotnet run <option> <args>");
        Console.WriteLine("\nOptions:");
        Console.WriteLine("  Cipher <seed> <tap>                    - Simulate one step of the LFSR cipher");
        Console.WriteLine("  GenerateKeystream <seed> <tap> <steps> - Generate a keystream of specified steps");
        Console.WriteLine("  Encrypt <plaintext>                    - Encrypt plaintext using the keystream");
        Console.WriteLine("  Decrypt <ciphertext>                   - Decrypt ciphertext using the keystream");
        Console.WriteLine("  TripleBits <seed> <tap> <step> <iteration> - Perform triple bits operation");
        Console.WriteLine("  EncryptImage <imagefile> <seed> <tap>  - Encrypt an image");
        Console.WriteLine("  DecryptImage <imagefile> <seed> <tap>  - Decrypt an image");
    }

    /// <summary>
    /// Runs the Cipher operation: performs one LFSR step on the provided seed and tap, then prints the updated state and generated bit.
    /// </summary>
    static void RunCipher(string seed, int tap)
    {
        if (tap >= seed.Length)
        {
            Console.WriteLine("Error: Tap position must be less than the seed length.");
            return;
        }

        Console.WriteLine($"{seed} – seed");
        LFSR lfsr = new LFSR(seed, tap);
        int bit = lfsr.Step();
        Console.WriteLine($"{lfsr.GetState()} {bit}");
    }

    /// <summary>
    /// Generates a keystream by performing a given number of LFSR steps, printing each updated state and bit,
    /// then writing the resulting keystream to a file named "keystream".
    /// </summary>
    static void RunGenerateKeystream(string seed, int tap, int steps)
    {
        if (tap >= seed.Length)
        {
            Console.WriteLine("Error: Tap position must be less than the seed length.");
            return;
        }

        Console.WriteLine($"{seed} – seed");
        LFSR lfsr = new LFSR(seed, tap);
        string keystream = "";

        for (int i = 0; i < steps; i++)
        {
            int bit = lfsr.Step();
            Console.WriteLine($"{lfsr.GetState()} {bit}");
            keystream += bit;
        }

        File.WriteAllText("keystream", keystream);
        Console.WriteLine($"The Keystream: {keystream}");
    }

    /// <summary>
    /// Encrypts a plaintext string by XORing it (bitwise) with a keystream read from a file.
    /// The process is done right-to-left with 0 padding.
    /// </summary>
    static void RunEncrypt(string plaintext)
    {
        if (!File.Exists("keystream"))
        {
            Console.WriteLine("Error: Keystream file not found. Run GenerateKeystream first.");
            return;
        }

        string keystream = File.ReadAllText("keystream").Trim();
        
        int cipherLength = Math.Max(plaintext.Length, keystream.Length);
        string ciphertext = "";
        
        for (int i = 0; i < cipherLength; i++)
        {
            int pIndex = plaintext.Length - 1 - i;
            int kIndex = keystream.Length - 1 - i;
            int pBit = (pIndex >= 0) ? plaintext[pIndex] - '0' : 0;
            int kBit = (kIndex >= 0) ? keystream[kIndex] - '0' : 0;
            int result = pBit ^ kBit;
            ciphertext = result.ToString() + ciphertext;
        }
        
        Console.WriteLine($"The ciphertext is: {ciphertext}");
    }

    /// <summary>
    /// Decrypts a ciphertext string by XORing it (bitwise) with a keystream read from a file.
    /// The process is done right-to-left with 0 padding.
    /// </summary>
    static void RunDecrypt(string ciphertext)
    {
        if (!File.Exists("keystream"))
        {
            Console.WriteLine("Error: Keystream file not found. Run GenerateKeystream first.");
            return;
        }

        string keystream = File.ReadAllText("keystream").Trim();
        
        // Determine the final plaintext length as the maximum of the two lengths.
        int plainLength = Math.Max(ciphertext.Length, keystream.Length);
        string plaintext = "";
        
        // Process bits from right to left.
        for (int i = 0; i < plainLength; i++)
        {
            int cIndex = ciphertext.Length - 1 - i;
            int kIndex = keystream.Length - 1 - i;
            int cBit = (cIndex >= 0) ? ciphertext[cIndex] - '0' : 0;
            int kBit = (kIndex >= 0) ? keystream[kIndex] - '0' : 0;
            int result = cBit ^ kBit;
            plaintext = result.ToString() + plaintext;
        }
        
        Console.WriteLine($"The plaintext is: {plaintext}");
    }

    /// <summary>
    /// Performs the TripleBits operation by running a given number of LFSR steps in each iteration,
    /// accumulating a value (accumulator * 3 + new bit) for every iteration and printing the final state
    /// and accumulated value.
    /// </summary>
    static void RunTripleBits(string seed, int tap, int steps, int iterations)
    {
        if (tap >= seed.Length)
        {
            Console.WriteLine("Error: Tap position must be less than the seed length.");
            return;
        }

        Console.WriteLine($"{seed} – seed");
        LFSR lfsr = new LFSR(seed, tap);

        for (int i = 0; i < iterations; i++)
        {
            int accumulatedValue = 1;

            for (int j = 0; j < steps; j++)
            {
                int bit = lfsr.Step();
                accumulatedValue = accumulatedValue * 3 + bit;
            }

            Console.WriteLine($"{lfsr.GetState()} {accumulatedValue}");
        }
    }

    /// <summary>
    /// Encrypts an image by performing one LFSR step on the seed to update it, converting the new state to an integer,
    /// creating a System.Random object then using the Random object to generate new random bytes for
    /// each color red, green, blue of each pixel.
    /// </summary>
    static void RunEncryptImageUsingRandom(string imageFile, string seed, int tap)
    {
        if (!File.Exists(imageFile))
        {
            Console.WriteLine($"Error: Image file '{imageFile}' not found.");
            return;
        }

        if (tap >= seed.Length)
        {
            Console.WriteLine("Error: Tap position must be less than the seed length.");
            return;
        }

        try
        {
            LFSR lfsr = new LFSR(seed, tap);
            lfsr.Step();  // Update the LFSR's state.

            int newSeedInt = Convert.ToInt32(lfsr.GetState(), 2);

            Random rng = new Random(newSeedInt);

            using (SKBitmap originalBitmap = SKBitmap.Decode(imageFile))
            {
                using (SKBitmap encryptedBitmap = new SKBitmap(originalBitmap.Width, originalBitmap.Height))
                {
                    for (int y = 0; y < originalBitmap.Height; y++)
                    {
                        for (int x = 0; x < originalBitmap.Width; x++)
                        {
                            SKColor originalColor = originalBitmap.GetPixel(x, y);

                            byte rRand = (byte)rng.Next(0, 256);
                            byte gRand = (byte)rng.Next(0, 256);
                            byte bRand = (byte)rng.Next(0, 256);

                            byte newR = (byte)(originalColor.Red ^ rRand);
                            byte newG = (byte)(originalColor.Green ^ gRand);
                            byte newB = (byte)(originalColor.Blue ^ bRand);

                            SKColor encryptedColor = new SKColor(newR, newG, newB, originalColor.Alpha);
                            encryptedBitmap.SetPixel(x, y, encryptedColor);
                        }
                    }

                    string fileName = Path.GetFileNameWithoutExtension(imageFile);
                    string extension = Path.GetExtension(imageFile);
                    string outputFile = $"{fileName}ENCRYPTED{extension}";

                    using (FileStream fs = new FileStream(outputFile, FileMode.Create))
                    {
                        SKImage image = SKImage.FromBitmap(encryptedBitmap);
                        SKData data = image.Encode(GetEncoderFromExtension(extension), 100);
                        data.SaveTo(fs);
                    }

                    Console.WriteLine($"Encrypted image saved as {outputFile}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error encrypting image: {ex.Message}");
        }
    }

    /// <summary>
    /// Decrypts an image by creating an LFSR from the provided seed and tap, performing one step to update it,
    /// converting the updated state to an integer.
    /// The method then uses the Random object to generate the same sequence of random bytes to XOR
    /// and recover the original image.
    /// </summary>
static void RunDecryptImageUsingRandom(string imageFile, string seed, int tap)
    {
        if (!File.Exists(imageFile))
        {
            Console.WriteLine($"Error: Image file '{imageFile}' not found.");
            return;
        }

        if (tap >= seed.Length)
        {
            Console.WriteLine("Error: Tap position must be less than the seed length.");
            return;
        }

        try
        {
            LFSR lfsr = new LFSR(seed, tap);
            lfsr.Step();
            
            int newSeedInt = Convert.ToInt32(lfsr.GetState(), 2);
            Random rng = new Random(newSeedInt);
            
            using (SKBitmap encryptedBitmap = SKBitmap.Decode(imageFile))
            {
                using (SKBitmap decryptedBitmap = new SKBitmap(encryptedBitmap.Width, encryptedBitmap.Height))
                {
                    for (int y = 0; y < encryptedBitmap.Height; y++)
                    {
                        for (int x = 0; x < encryptedBitmap.Width; x++)
                        {
                            SKColor encColor = encryptedBitmap.GetPixel(x, y);

                            byte rRand = (byte)rng.Next(0, 256);
                            byte gRand = (byte)rng.Next(0, 256);
                            byte bRand = (byte)rng.Next(0, 256);

                            byte origR = (byte)(encColor.Red ^ rRand);
                            byte origG = (byte)(encColor.Green ^ gRand);
                            byte origB = (byte)(encColor.Blue ^ bRand);

                            SKColor decColor = new SKColor(origR, origG, origB, encColor.Alpha);
                            decryptedBitmap.SetPixel(x, y, decColor);
                        }
                    }

                    string fileName = Path.GetFileNameWithoutExtension(imageFile);
                    string extension = Path.GetExtension(imageFile);
                    if (fileName.EndsWith("ENCRYPTED", StringComparison.OrdinalIgnoreCase))
                    {
                        fileName = fileName.Substring(0, fileName.Length - 9);
                    }
                    string outputFile = $"{fileName}NEW{extension}";

                    using (FileStream fs = new FileStream(outputFile, FileMode.Create))
                    {
                        SKImage image = SKImage.FromBitmap(decryptedBitmap);
                        SKData data = image.Encode(GetEncoderFromExtension(extension), 100);
                        data.SaveTo(fs);
                    }
                    Console.WriteLine($"Decrypted image saved as {outputFile}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error decrypting image: {ex.Message}");
        }
    }

    /// <summary>
    /// Chooses the appropriate SkiaSharp image encoder based on the file extension.
    /// </summary>
    static SKEncodedImageFormat GetEncoderFromExtension(string extension)
    {
        switch (extension.ToLower())
        {
            case ".jpg":
            case ".jpeg":
                return SKEncodedImageFormat.Jpeg;
            case ".png":
                return SKEncodedImageFormat.Png;
            case ".gif":
                return SKEncodedImageFormat.Gif;
            case ".bmp":
                return SKEncodedImageFormat.Bmp;
            case ".webp":
                return SKEncodedImageFormat.Webp;
            default:
                return SKEncodedImageFormat.Png;
        }
    }
}