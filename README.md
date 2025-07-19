# 🔐 LFSR Stream Cipher & Image Encryptor

A C# implementation of a Linear Feedback Shift Register (LFSR)-based stream cipher for encrypting text and images. Includes several utilities such as keystream generation, triple-bit computation, and image encryption/decryption using SkiaSharp.

---

## 📌 Project Overview

This project simulates the use of LFSR (Linear Feedback Shift Register) for generating pseudorandom sequences to:

- Encrypt/Decrypt binary strings
- Encrypt/Decrypt images using generated pseudorandom bytes
- Demonstrate bit manipulation operations like `TripleBits`

---

## 🛠 Features

- 🔁 Simulate one LFSR step with a seed and tap
- 🔑 Generate a keystream of pseudorandom bits
- ✉️ Encrypt/Decrypt binary strings using XOR and keystream
- 🖼️ Encrypt/Decrypt images (PNG, JPG, etc.)
- 🧮 Compute a custom "TripleBits" accumulator operation
- 🧾 Simple CLI with usage help

---

## 💻 Technologies Used

- C# (.NET Core)
- SkiaSharp (image processing)
- System.IO (file management)

---

## 🚀 How to Run

1. **Build and run the project:**

```bash
dotnet build
dotnet run <option> <arguments>
```

2. **Available Commands:

| Command                              | Example                                                    | Description                      |
| ------------------------------------ | ---------------------------------------------------------- | -------------------------------- |
| `Cipher <seed> <tap>`                | `dotnet run Cipher 01101000010 8`                          | Perform one LFSR step            |
| `GenerateKeystream <seed> <tap> <n>` | `dotnet run GenerateKeystream 01101000010 8 10`            | Generate keystream of n bits     |
| `Encrypt <plaintext>`                | `dotnet run Encrypt 1010101`                               | Encrypt a binary string          |
| `Decrypt <ciphertext>`               | `dotnet run Decrypt 0110110`                               | Decrypt a binary string          |
| `TripleBits <seed> <tap> <s> <iter>` | `dotnet run TripleBits 01101000010 8 5 3`                  | Run custom triple-bits operation |
| `EncryptImage <file> <seed> <tap>`   | `dotnet run EncryptImage image.png 01101000010 8`          | Encrypt an image                 |
| `DecryptImage <file> <seed> <tap>`   | `dotnet run DecryptImage imageENCRYPTED.png 01101000010 8` | Decrypt an encrypted image       |

---

## 🧪 Example Output

```bash
> dotnet run Cipher 01101000010 8
01101000010 – seed
11010000101 1
```

---

## 📂 File Structure

```bash
├── Program.cs         // Main logic for CLI and image handling
├── LFSR.cs            // LFSR logic class
├── keystream          // (Generated) Contains binary keystream
```

---

## 📸 Image Encryption Example
- Input: photo.png
- Output after encryption: photoENCRYPTED.png
- Output after decryption: photoNEW.png

Supported formats: PNG, JPG, BMP, GIF, WEBP
Encryption is done using XOR on RGB channels with a pseudorandom sequence.

---

## 📝 License
This project is for educational purposes. Free to use, share, and modify.
No warranties or guarantees are provided.

---

## 👨‍💻 Author
Aniruddha Roy
Rochester Institute of Technology
Let me know if you want a `demo/` folder for sample inputs and outputs, or badges for GitHub Actions, license, etc.



