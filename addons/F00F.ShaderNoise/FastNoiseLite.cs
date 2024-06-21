// MIT License
//
// Copyright(c) 2023 Jordan Peck (jordan.me2@gmail.com)
// Copyright(c) 2023 Contributors
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
// .'',;:cldxkO00KKXXNNWWWNNXKOkxdollcc::::::;:::ccllloooolllllllllooollc:,'...        ...........',;cldxkO000Okxdlc::;;;,,;;;::cclllllll
// ..',;:ldxO0KXXNNNNNNNNXXK0kxdolcc::::::;;;,,,,,,;;;;;;;;;;:::cclllllc:;'....       ...........',;:ldxO0KXXXK0Okxdolc::;;;;::cllodddddo
// ...',:loxO0KXNNNNNXXKK0Okxdolc::;::::::::;;;,,'''''.....''',;:clllllc:;,'............''''''''',;:loxO0KXNNNNNXK0Okxdollccccllodxxxxxxd
// ....';:ldkO0KXXXKK00Okxdolcc:;;;;;::cclllcc:;;,''..... ....',;clooddolcc:;;;;,,;;;;;::::;;;;;;:cloxk0KXNWWWWWWNXKK0Okxddoooddxxkkkkkxx
// .....';:ldxkOOOOOkxxdolcc:;;;,,,;;:cllooooolcc:;'...      ..,:codxkkkxddooollloooooooollcc:::::clodkO0KXNWWWWWWNNXK00Okxxxxxxxxkkkkxxx
// . ....';:cloddddo___________,,,,;;:clooddddoolc:,...      ..,:ldx__00OOOkkk___kkkkkkxxdollc::::cclodkO0KXXNNNNNNXXK0OOkxxxxxxxxxxxxddd
// .......',;:cccc:|           |,,,;;:cclooddddoll:;'..     ..';cox|  \KKK000|   |KK00OOkxdocc___;::clldxxkO0KKKKK00Okkxdddddddddddddddoo
// .......'',,,,,''|   ________|',,;;::cclloooooolc:;'......___:ldk|   \KK000|   |XKKK0Okxolc|   |;;::cclodxxkkkkxxdoolllcclllooodddooooo
// ''......''''....|   |  ....'',,,,;;;::cclloooollc:;,''.'|   |oxk|    \OOO0|   |KKK00Oxdoll|___|;;;;;::ccllllllcc::;;,,;;;:cclloooooooo
// ;;,''.......... |   |_____',,;;;____:___cllo________.___|   |___|     \xkk|   |KK_______ool___:::;________;;;_______...'',;;:ccclllloo
// c:;,''......... |         |:::/     '   |lo/        |           |      \dx|   |0/       \d|   |cc/        |'/       \......',,;;:ccllo
// ol:;,'..........|    _____|ll/    __    |o/   ______|____    ___|   |   \o|   |/   ___   \|   |o/   ______|/   ___   \ .......'',;:clo
// dlc;,...........|   |::clooo|    /  |   |x\___   \KXKKK0|   |dol|   |\   \|   |   |   |   |   |d\___   \..|   |  /   /       ....',:cl
// xoc;'...  .....'|   |llodddd|    \__|   |_____\   \KKK0O|   |lc:|   |'\       |   |___|   |   |_____\   \.|   |_/___/...      ...',;:c
// dlc;'... ....',;|   |oddddddo\          |          |Okkx|   |::;|   |..\      |\         /|   |          | \         |...    ....',;:c
// ol:,'.......',:c|___|xxxddollc\_____,___|_________/ddoll|___|,,,|___|...\_____|:\ ______/l|___|_________/...\________|'........',;::cc
// c:;'.......';:codxxkkkkxxolc::;::clodxkOO0OOkkxdollc::;;,,''''',,,,''''''''''',,'''''',;:loxkkOOkxol:;,'''',,;:ccllcc:;,'''''',;::ccll
// ;,'.......',:codxkOO0OOkxdlc:;,,;;:cldxxkkxxdolc:;;,,''.....'',;;:::;;,,,'''''........,;cldkO0KK0Okdoc::;;::cloodddoolc:;;;;;::ccllooo
// .........',;:lodxOO0000Okdoc:,,',,;:clloddoolc:;,''.......'',;:clooollc:;;,,''.......',:ldkOKXNNXX0Oxdolllloddxxxxxxdolccccccllooodddd
// .    .....';:cldxkO0000Okxol:;,''',,;::cccc:;,,'.......'',;:cldxxkkxxdolc:;;,'.......';coxOKXNWWWNXKOkxddddxxkkkkkkxdoollllooddxxxxkkk
//       ....',;:codxkO000OOxdoc:;,''',,,;;;;,''.......',,;:clodkO00000Okxolc::;,,''..',;:ldxOKXNWWWNNK0OkkkkkkkkkkkxxddooooodxxkOOOOO000
//       ....',;;clodxkkOOOkkdolc:;,,,,,,,,'..........,;:clodxkO0KKXKK0Okxdolcc::;;,,,;;:codkO0XXNNNNXKK0OOOOOkkkkxxdoollloodxkO0KKKXXXXX
//
// VERSION: 1.1.0 (modified)
// https://github.com/Auburn/FastNoiseLite

using System;
using System.Runtime.CompilerServices;

// Switch between using floats or doubles for input position
using FNLfloat = System.Single;
//using FNLfloat = System.Double;

namespace F00F.ShaderNoise
{
    public class FastNoiseLite
    {
        private const short INLINE = (short)MethodImplOptions.AggressiveInlining;
        private const short OPTIMISE = (short)MethodImplOptions.AggressiveOptimization;

        public enum NoiseType 
        {
            Perlin,
            ValueCubic,
            Value,
        };

        public enum FractalType 
        {
            None,
            FBm,
            Ridged,
            PingPong,
        };

        public enum DomainWarp
        {
            None,
            Single,
            Independent,
            Progressive,
        };

        public enum DomainWarpType
        {
            BasicGrid,
        };

        private int mSeed = 0;
        private float mFrequency = 0.01f;
        private NoiseType mNoiseType = NoiseType.Perlin;

        private FractalType mFractalType = FractalType.None;
        private int mOctaves = 3;
        private float mLacunarity = 2.0f;
        private float mGain = 0.5f;
        private float mWeightedStrength = 0.0f;
        private float mPingPongStrength = 2.0f;

        private float mFractalBounding = 1 / 1.75f;

        private DomainWarp mDomainWarp = DomainWarp.None;
        private DomainWarpType mDomainWarpType = DomainWarpType.BasicGrid;
        private float mDomainWarpAmp = 1.0f;

        /// <summary>
        /// Create new FastNoise object with optional seed
        /// </summary>
        public FastNoiseLite(int seed = 0)
            => SetSeed(seed);

        /// <summary>
        /// Sets seed used for all noise types
        /// </summary>
        /// <remarks>
        /// Default: 0
        /// </remarks>
        public void SetSeed(int seed)
            => mSeed = seed;

        /// <summary>
        /// Sets frequency for all noise types
        /// </summary>
        /// <remarks>
        /// Default: 0.01
        /// </remarks>
        public void SetFrequency(float frequency)
            => mFrequency = frequency;

        /// <summary>
        /// Sets noise algorithm used for GetNoise(...)
        /// </summary>
        /// <remarks>
        /// Default: Perlin
        /// </remarks>
        public void SetNoiseType(NoiseType noiseType)
            => mNoiseType = noiseType;

        /// <summary>
        /// Sets method for combining octaves in all fractal noise types
        /// </summary>
        /// <remarks>
        /// Default: None
        /// </remarks>
        public void SetFractalType(FractalType fractalType)
            => mFractalType = fractalType;

        /// <summary>
        /// Sets octave count for all fractal noise types
        /// </summary>
        /// <remarks>
        /// Default: 3
        /// </remarks>
        public void SetFractalOctaves(int octaves)
        {
            mOctaves = octaves;
            CalculateFractalBounding();
        }

        /// <summary>
        /// Sets octave lacunarity for all fractal noise types
        /// </summary>
        /// <remarks>
        /// Default: 2.0
        /// </remarks>
        public void SetFractalLacunarity(float lacunarity)
            => mLacunarity = lacunarity;

        /// <summary>
        /// Sets octave gain for all fractal noise types
        /// </summary>
        /// <remarks>
        /// Default: 0.5
        /// </remarks>
        public void SetFractalGain(float gain)
        {
            mGain = gain;
            CalculateFractalBounding();
        }

        /// <summary>
        /// Sets octave weighting for all fractal noise types
        /// </summary>
        /// <remarks>
        /// Default: 0.0
        /// Note: Keep between 0...1 to maintain -1...1 output bounding
        /// </remarks>
        public void SetFractalWeightedStrength(float weightedStrength)
            => mWeightedStrength = weightedStrength;

        /// <summary>
        /// Sets strength of the fractal ping pong effect
        /// </summary>
        /// <remarks>
        /// Default: 2.0
        /// </remarks>
        public void SetFractalPingPongStrength(float pingPongStrength)
            => mPingPongStrength = pingPongStrength;

        /// <summary>
        /// Sets how to apply the Domain Warp algorithm
        /// </summary>
        /// <remarks>
        /// Default: None
        /// </remarks>
        public void SetDomainWarp(DomainWarp domainWarp)
            => mDomainWarp = domainWarp;

        /// <summary>
        /// Sets the warp algorithm when using DomainWarp
        /// </summary>
        /// <remarks>
        /// Default: BasicGrid
        /// </remarks>
        public void SetDomainWarpType(DomainWarpType domainWarpType)
            => mDomainWarpType = domainWarpType;

        /// <summary>
        /// Sets the maximum warp distance from original position when using DomainWarp
        /// </summary>
        /// <remarks>
        /// Default: 1.0
        /// </remarks>
        public void SetDomainWarpAmp(float domainWarpAmp)
            => mDomainWarpAmp = domainWarpAmp;

        /// <summary>
        /// 2D noise at given position using current settings
        /// </summary>
        /// <returns>
        /// Noise output bounded between -1...1
        /// </returns>
        [MethodImpl(OPTIMISE)]
        public float GetNoise(FNLfloat x, FNLfloat y)
        {
            switch (mDomainWarp)
            {
                case DomainWarp.Single:
                    DomainWarpSingle(ref x, ref y);
                    break;
                case DomainWarp.Independent:
                    DomainWarpIndependent(ref x, ref y);
                    break;
                case DomainWarp.Progressive:
                    DomainWarpProgressive(ref x, ref y);
                    break;
            }

            x *= mFrequency;
            y *= mFrequency;

            switch (mFractalType)
            {
                case FractalType.FBm:
                    return GenFractalFBm(x, y);
                case FractalType.Ridged:
                    return GenFractalRidged(x, y);
                case FractalType.PingPong:
                    return GenFractalPingPong(x, y);
                default:
                    return GenNoiseSingle(mSeed, x, y);
            }
        }

        private static readonly float[] Gradients2D =
        {
             0.130526192220052f,  0.99144486137381f,   0.38268343236509f,   0.923879532511287f,  0.608761429008721f,  0.793353340291235f,  0.793353340291235f,  0.608761429008721f,
             0.923879532511287f,  0.38268343236509f,   0.99144486137381f,   0.130526192220051f,  0.99144486137381f,  -0.130526192220051f,  0.923879532511287f, -0.38268343236509f,
             0.793353340291235f, -0.60876142900872f,   0.608761429008721f, -0.793353340291235f,  0.38268343236509f,  -0.923879532511287f,  0.130526192220052f, -0.99144486137381f,
            -0.130526192220052f, -0.99144486137381f,  -0.38268343236509f,  -0.923879532511287f, -0.608761429008721f, -0.793353340291235f, -0.793353340291235f, -0.608761429008721f,
            -0.923879532511287f, -0.38268343236509f,  -0.99144486137381f,  -0.130526192220052f, -0.99144486137381f,   0.130526192220051f, -0.923879532511287f,  0.38268343236509f,
            -0.793353340291235f,  0.608761429008721f, -0.608761429008721f,  0.793353340291235f, -0.38268343236509f,   0.923879532511287f, -0.130526192220052f,  0.99144486137381f,
             0.130526192220052f,  0.99144486137381f,   0.38268343236509f,   0.923879532511287f,  0.608761429008721f,  0.793353340291235f,  0.793353340291235f,  0.608761429008721f,
             0.923879532511287f,  0.38268343236509f,   0.99144486137381f,   0.130526192220051f,  0.99144486137381f,  -0.130526192220051f,  0.923879532511287f, -0.38268343236509f,
             0.793353340291235f, -0.60876142900872f,   0.608761429008721f, -0.793353340291235f,  0.38268343236509f,  -0.923879532511287f,  0.130526192220052f, -0.99144486137381f,
            -0.130526192220052f, -0.99144486137381f,  -0.38268343236509f,  -0.923879532511287f, -0.608761429008721f, -0.793353340291235f, -0.793353340291235f, -0.608761429008721f,
            -0.923879532511287f, -0.38268343236509f,  -0.99144486137381f,  -0.130526192220052f, -0.99144486137381f,   0.130526192220051f, -0.923879532511287f,  0.38268343236509f,
            -0.793353340291235f,  0.608761429008721f, -0.608761429008721f,  0.793353340291235f, -0.38268343236509f,   0.923879532511287f, -0.130526192220052f,  0.99144486137381f,
             0.130526192220052f,  0.99144486137381f,   0.38268343236509f,   0.923879532511287f,  0.608761429008721f,  0.793353340291235f,  0.793353340291235f,  0.608761429008721f,
             0.923879532511287f,  0.38268343236509f,   0.99144486137381f,   0.130526192220051f,  0.99144486137381f,  -0.130526192220051f,  0.923879532511287f, -0.38268343236509f,
             0.793353340291235f, -0.60876142900872f,   0.608761429008721f, -0.793353340291235f,  0.38268343236509f,  -0.923879532511287f,  0.130526192220052f, -0.99144486137381f,
            -0.130526192220052f, -0.99144486137381f,  -0.38268343236509f,  -0.923879532511287f, -0.608761429008721f, -0.793353340291235f, -0.793353340291235f, -0.608761429008721f,
            -0.923879532511287f, -0.38268343236509f,  -0.99144486137381f,  -0.130526192220052f, -0.99144486137381f,   0.130526192220051f, -0.923879532511287f,  0.38268343236509f,
            -0.793353340291235f,  0.608761429008721f, -0.608761429008721f,  0.793353340291235f, -0.38268343236509f,   0.923879532511287f, -0.130526192220052f,  0.99144486137381f,
             0.130526192220052f,  0.99144486137381f,   0.38268343236509f,   0.923879532511287f,  0.608761429008721f,  0.793353340291235f,  0.793353340291235f,  0.608761429008721f,
             0.923879532511287f,  0.38268343236509f,   0.99144486137381f,   0.130526192220051f,  0.99144486137381f,  -0.130526192220051f,  0.923879532511287f, -0.38268343236509f,
             0.793353340291235f, -0.60876142900872f,   0.608761429008721f, -0.793353340291235f,  0.38268343236509f,  -0.923879532511287f,  0.130526192220052f, -0.99144486137381f,
            -0.130526192220052f, -0.99144486137381f,  -0.38268343236509f,  -0.923879532511287f, -0.608761429008721f, -0.793353340291235f, -0.793353340291235f, -0.608761429008721f,
            -0.923879532511287f, -0.38268343236509f,  -0.99144486137381f,  -0.130526192220052f, -0.99144486137381f,   0.130526192220051f, -0.923879532511287f,  0.38268343236509f,
            -0.793353340291235f,  0.608761429008721f, -0.608761429008721f,  0.793353340291235f, -0.38268343236509f,   0.923879532511287f, -0.130526192220052f,  0.99144486137381f,
             0.130526192220052f,  0.99144486137381f,   0.38268343236509f,   0.923879532511287f,  0.608761429008721f,  0.793353340291235f,  0.793353340291235f,  0.608761429008721f,
             0.923879532511287f,  0.38268343236509f,   0.99144486137381f,   0.130526192220051f,  0.99144486137381f,  -0.130526192220051f,  0.923879532511287f, -0.38268343236509f,
             0.793353340291235f, -0.60876142900872f,   0.608761429008721f, -0.793353340291235f,  0.38268343236509f,  -0.923879532511287f,  0.130526192220052f, -0.99144486137381f,
            -0.130526192220052f, -0.99144486137381f,  -0.38268343236509f,  -0.923879532511287f, -0.608761429008721f, -0.793353340291235f, -0.793353340291235f, -0.608761429008721f,
            -0.923879532511287f, -0.38268343236509f,  -0.99144486137381f,  -0.130526192220052f, -0.99144486137381f,   0.130526192220051f, -0.923879532511287f,  0.38268343236509f,
            -0.793353340291235f,  0.608761429008721f, -0.608761429008721f,  0.793353340291235f, -0.38268343236509f,   0.923879532511287f, -0.130526192220052f,  0.99144486137381f,
             0.38268343236509f,   0.923879532511287f,  0.923879532511287f,  0.38268343236509f,   0.923879532511287f, -0.38268343236509f,   0.38268343236509f,  -0.923879532511287f,
            -0.38268343236509f,  -0.923879532511287f, -0.923879532511287f, -0.38268343236509f,  -0.923879532511287f,  0.38268343236509f,  -0.38268343236509f,   0.923879532511287f,
        };

        private static readonly float[] RandVecs2D =
        {
            -0.2700222198f, -0.9628540911f, 0.3863092627f, -0.9223693152f, 0.04444859006f, -0.999011673f, -0.5992523158f, -0.8005602176f, -0.7819280288f, 0.6233687174f, 0.9464672271f, 0.3227999196f, -0.6514146797f, -0.7587218957f, 0.9378472289f, 0.347048376f,
            -0.8497875957f, -0.5271252623f, -0.879042592f, 0.4767432447f, -0.892300288f, -0.4514423508f, -0.379844434f, -0.9250503802f, -0.9951650832f, 0.0982163789f, 0.7724397808f, -0.6350880136f, 0.7573283322f, -0.6530343002f, -0.9928004525f, -0.119780055f,
            -0.0532665713f, 0.9985803285f, 0.9754253726f, -0.2203300762f, -0.7665018163f, 0.6422421394f, 0.991636706f, 0.1290606184f, -0.994696838f, 0.1028503788f, -0.5379205513f, -0.84299554f, 0.5022815471f, -0.8647041387f, 0.4559821461f, -0.8899889226f,
            -0.8659131224f, -0.5001944266f, 0.0879458407f, -0.9961252577f, -0.5051684983f, 0.8630207346f, 0.7753185226f, -0.6315704146f, -0.6921944612f, 0.7217110418f, -0.5191659449f, -0.8546734591f, 0.8978622882f, -0.4402764035f, -0.1706774107f, 0.9853269617f,
            -0.9353430106f, -0.3537420705f, -0.9992404798f, 0.03896746794f, -0.2882064021f, -0.9575683108f, -0.9663811329f, 0.2571137995f, -0.8759714238f, -0.4823630009f, -0.8303123018f, -0.5572983775f, 0.05110133755f, -0.9986934731f, -0.8558373281f, -0.5172450752f,
            0.09887025282f, 0.9951003332f, 0.9189016087f, 0.3944867976f, -0.2439375892f, -0.9697909324f, -0.8121409387f, -0.5834613061f, -0.9910431363f, 0.1335421355f, 0.8492423985f, -0.5280031709f, -0.9717838994f, -0.2358729591f, 0.9949457207f, 0.1004142068f,
            0.6241065508f, -0.7813392434f, 0.662910307f, 0.7486988212f, -0.7197418176f, 0.6942418282f, -0.8143370775f, -0.5803922158f, 0.104521054f, -0.9945226741f, -0.1065926113f, -0.9943027784f, 0.445799684f, -0.8951327509f, 0.105547406f, 0.9944142724f,
            -0.992790267f, 0.1198644477f, -0.8334366408f, 0.552615025f, 0.9115561563f, -0.4111755999f, 0.8285544909f, -0.5599084351f, 0.7217097654f, -0.6921957921f, 0.4940492677f, -0.8694339084f, -0.3652321272f, -0.9309164803f, -0.9696606758f, 0.2444548501f,
            0.08925509731f, -0.996008799f, 0.5354071276f, -0.8445941083f, -0.1053576186f, 0.9944343981f, -0.9890284586f, 0.1477251101f, 0.004856104961f, 0.9999882091f, 0.9885598478f, 0.1508291331f, 0.9286129562f, -0.3710498316f, -0.5832393863f, -0.8123003252f,
            0.3015207509f, 0.9534596146f, -0.9575110528f, 0.2883965738f, 0.9715802154f, -0.2367105511f, 0.229981792f, 0.9731949318f, 0.955763816f, -0.2941352207f, 0.740956116f, 0.6715534485f, -0.9971513787f, -0.07542630764f, 0.6905710663f, -0.7232645452f,
            -0.290713703f, -0.9568100872f, 0.5912777791f, -0.8064679708f, -0.9454592212f, -0.325740481f, 0.6664455681f, 0.74555369f, 0.6236134912f, 0.7817328275f, 0.9126993851f, -0.4086316587f, -0.8191762011f, 0.5735419353f, -0.8812745759f, -0.4726046147f,
            0.9953313627f, 0.09651672651f, 0.9855650846f, -0.1692969699f, -0.8495980887f, 0.5274306472f, 0.6174853946f, -0.7865823463f, 0.8508156371f, 0.52546432f, 0.9985032451f, -0.05469249926f, 0.1971371563f, -0.9803759185f, 0.6607855748f, -0.7505747292f,
            -0.03097494063f, 0.9995201614f, -0.6731660801f, 0.739491331f, -0.7195018362f, -0.6944905383f, 0.9727511689f, 0.2318515979f, 0.9997059088f, -0.0242506907f, 0.4421787429f, -0.8969269532f, 0.9981350961f, -0.061043673f, -0.9173660799f, -0.3980445648f,
            -0.8150056635f, -0.5794529907f, -0.8789331304f, 0.4769450202f, 0.0158605829f, 0.999874213f, -0.8095464474f, 0.5870558317f, -0.9165898907f, -0.3998286786f, -0.8023542565f, 0.5968480938f, -0.5176737917f, 0.8555780767f, -0.8154407307f, -0.5788405779f,
            0.4022010347f, -0.9155513791f, -0.9052556868f, -0.4248672045f, 0.7317445619f, 0.6815789728f, -0.5647632201f, -0.8252529947f, -0.8403276335f, -0.5420788397f, -0.9314281527f, 0.363925262f, 0.5238198472f, 0.8518290719f, 0.7432803869f, -0.6689800195f,
            -0.985371561f, -0.1704197369f, 0.4601468731f, 0.88784281f, 0.825855404f, 0.5638819483f, 0.6182366099f, 0.7859920446f, 0.8331502863f, -0.553046653f, 0.1500307506f, 0.9886813308f, -0.662330369f, -0.7492119075f, -0.668598664f, 0.743623444f,
            0.7025606278f, 0.7116238924f, -0.5419389763f, -0.8404178401f, -0.3388616456f, 0.9408362159f, 0.8331530315f, 0.5530425174f, -0.2989720662f, -0.9542618632f, 0.2638522993f, 0.9645630949f, 0.124108739f, -0.9922686234f, -0.7282649308f, -0.6852956957f,
            0.6962500149f, 0.7177993569f, -0.9183535368f, 0.3957610156f, -0.6326102274f, -0.7744703352f, -0.9331891859f, -0.359385508f, -0.1153779357f, -0.9933216659f, 0.9514974788f, -0.3076565421f, -0.08987977445f, -0.9959526224f, 0.6678496916f, 0.7442961705f,
            0.7952400393f, -0.6062947138f, -0.6462007402f, -0.7631674805f, -0.2733598753f, 0.9619118351f, 0.9669590226f, -0.254931851f, -0.9792894595f, 0.2024651934f, -0.5369502995f, -0.8436138784f, -0.270036471f, -0.9628500944f, -0.6400277131f, 0.7683518247f,
            -0.7854537493f, -0.6189203566f, 0.06005905383f, -0.9981948257f, -0.02455770378f, 0.9996984141f, -0.65983623f, 0.751409442f, -0.6253894466f, -0.7803127835f, -0.6210408851f, -0.7837781695f, 0.8348888491f, 0.5504185768f, -0.1592275245f, 0.9872419133f,
            0.8367622488f, 0.5475663786f, -0.8675753916f, -0.4973056806f, -0.2022662628f, -0.9793305667f, 0.9399189937f, 0.3413975472f, 0.9877404807f, -0.1561049093f, -0.9034455656f, 0.4287028224f, 0.1269804218f, -0.9919052235f, -0.3819600854f, 0.924178821f,
            0.9754625894f, 0.2201652486f, -0.3204015856f, -0.9472818081f, -0.9874760884f, 0.1577687387f, 0.02535348474f, -0.9996785487f, 0.4835130794f, -0.8753371362f, -0.2850799925f, -0.9585037287f, -0.06805516006f, -0.99768156f, -0.7885244045f, -0.6150034663f,
            0.3185392127f, -0.9479096845f, 0.8880043089f, 0.4598351306f, 0.6476921488f, -0.7619021462f, 0.9820241299f, 0.1887554194f, 0.9357275128f, -0.3527237187f, -0.8894895414f, 0.4569555293f, 0.7922791302f, 0.6101588153f, 0.7483818261f, 0.6632681526f,
            -0.7288929755f, -0.6846276581f, 0.8729032783f, -0.4878932944f, 0.8288345784f, 0.5594937369f, 0.08074567077f, 0.9967347374f, 0.9799148216f, -0.1994165048f, -0.580730673f, -0.8140957471f, -0.4700049791f, -0.8826637636f, 0.2409492979f, 0.9705377045f,
            0.9437816757f, -0.3305694308f, -0.8927998638f, -0.4504535528f, -0.8069622304f, 0.5906030467f, 0.06258973166f, 0.9980393407f, -0.9312597469f, 0.3643559849f, 0.5777449785f, 0.8162173362f, -0.3360095855f, -0.941858566f, 0.697932075f, -0.7161639607f,
            -0.002008157227f, -0.9999979837f, -0.1827294312f, -0.9831632392f, -0.6523911722f, 0.7578824173f, -0.4302626911f, -0.9027037258f, -0.9985126289f, -0.05452091251f, -0.01028102172f, -0.9999471489f, -0.4946071129f, 0.8691166802f, -0.2999350194f, 0.9539596344f,
            0.8165471961f, 0.5772786819f, 0.2697460475f, 0.962931498f, -0.7306287391f, -0.6827749597f, -0.7590952064f, -0.6509796216f, -0.907053853f, 0.4210146171f, -0.5104861064f, -0.8598860013f, 0.8613350597f, 0.5080373165f, 0.5007881595f, -0.8655698812f,
            -0.654158152f, 0.7563577938f, -0.8382755311f, -0.545246856f, 0.6940070834f, 0.7199681717f, 0.06950936031f, 0.9975812994f, 0.1702942185f, -0.9853932612f, 0.2695973274f, 0.9629731466f, 0.5519612192f, -0.8338697815f, 0.225657487f, -0.9742067022f,
            0.4215262855f, -0.9068161835f, 0.4881873305f, -0.8727388672f, -0.3683854996f, -0.9296731273f, -0.9825390578f, 0.1860564427f, 0.81256471f, 0.5828709909f, 0.3196460933f, -0.9475370046f, 0.9570913859f, 0.2897862643f, -0.6876655497f, -0.7260276109f,
            -0.9988770922f, -0.047376731f, -0.1250179027f, 0.992154486f, -0.8280133617f, 0.560708367f, 0.9324863769f, -0.3612051451f, 0.6394653183f, 0.7688199442f, -0.01623847064f, -0.9998681473f, -0.9955014666f, -0.09474613458f, -0.81453315f, 0.580117012f,
            0.4037327978f, -0.9148769469f, 0.9944263371f, 0.1054336766f, -0.1624711654f, 0.9867132919f, -0.9949487814f, -0.100383875f, -0.6995302564f, 0.7146029809f, 0.5263414922f, -0.85027327f, -0.5395221479f, 0.841971408f, 0.6579370318f, 0.7530729462f,
            0.01426758847f, -0.9998982128f, -0.6734383991f, 0.7392433447f, 0.639412098f, -0.7688642071f, 0.9211571421f, 0.3891908523f, -0.146637214f, -0.9891903394f, -0.782318098f, 0.6228791163f, -0.5039610839f, -0.8637263605f, -0.7743120191f, -0.6328039957f,
        };

        [MethodImpl(INLINE)]
        private static float FastMin(float a, float b) { return a < b ? a : b; }

        //[MethodImpl(INLINE)]
        //private static float FastMax(float a, float b) { return a > b ? a : b; }

        [MethodImpl(INLINE)]
        private static float FastAbs(float f) { return f < 0 ? -f : f; }

        //[MethodImpl(INLINE)]
        //private static float FastSqrt(float f) { return (float)Math.Sqrt(f); }

        [MethodImpl(INLINE)]
        private static int FastFloor(FNLfloat f) { return f >= 0 ? (int)f : (int)f - 1; }

        //[MethodImpl(INLINE)]
        //private static int FastRound(FNLfloat f) { return f >= 0 ? (int)(f + 0.5f) : (int)(f - 0.5f); }

        [MethodImpl(INLINE)]
        private static float Lerp(float a, float b, float t) { return a + t * (b - a); }

        [MethodImpl(INLINE)]
        private static float InterpHermite(float t) { return t * t * (3 - 2 * t); }

        [MethodImpl(INLINE)]
        private static float InterpQuintic(float t) { return t * t * t * (t * (t * 6 - 15) + 10); }

        [MethodImpl(INLINE)]
        private static float CubicLerp(float a, float b, float c, float d, float t)
        {
            float p = (d - c) - (a - b);
            return t * t * t * p + t * t * ((a - b) - p) + t * (c - a) + b;
        }

        [MethodImpl(INLINE)]
        private static float PingPong(float t)
        {
            t -= (int)(t * 0.5f) * 2;
            return t < 1 ? t : 2 - t;
        }

        private void CalculateFractalBounding()
        {
            float gain = FastAbs(mGain);
            float amp = gain;
            float ampFractal = 1.0f;
            for (int i = 1; i < mOctaves; i++)
            {
                ampFractal += amp;
                amp *= gain;
            }
            mFractalBounding = 1 / ampFractal;
        }

        // Hashing
        
        private const int PrimeX = 501125321;
        private const int PrimeY = 1136930381;

        [MethodImpl(INLINE)]
        private static int Hash(int seed, int xPrimed, int yPrimed)
        {
            int hash = seed ^ xPrimed ^ yPrimed;

            hash *= 0x27d4eb2d;
            return hash;
        }

        [MethodImpl(INLINE)]
        private static float ValCoord(int seed, int xPrimed, int yPrimed)
        {
            int hash = Hash(seed, xPrimed, yPrimed);

            hash *= hash;
            hash ^= hash << 19;
            return hash * (1 / 2147483648.0f);
        }

        [MethodImpl(INLINE)]
        private static float GradCoord(int seed, int xPrimed, int yPrimed, float xd, float yd)
        {
            int hash = Hash(seed, xPrimed, yPrimed);
            hash ^= hash >> 15;
            hash &= 127 << 1;

            float xg = Gradients2D[hash];
            float yg = Gradients2D[hash | 1];

            return xd * xg + yd * yg;
        }

        // Noise

        private float SinglePerlin(int seed, FNLfloat x, FNLfloat y)
        {
            int x0 = FastFloor(x);
            int y0 = FastFloor(y);

            float xd0 = (float)(x - x0);
            float yd0 = (float)(y - y0);
            float xd1 = xd0 - 1;
            float yd1 = yd0 - 1;

            float xs = InterpQuintic(xd0);
            float ys = InterpQuintic(yd0);

            x0 *= PrimeX;
            y0 *= PrimeY;
            int x1 = x0 + PrimeX;
            int y1 = y0 + PrimeY;

            float xf0 = Lerp(GradCoord(seed, x0, y0, xd0, yd0), GradCoord(seed, x1, y0, xd1, yd0), xs);
            float xf1 = Lerp(GradCoord(seed, x0, y1, xd0, yd1), GradCoord(seed, x1, y1, xd1, yd1), xs);

            return Lerp(xf0, xf1, ys) * 1.4247691104677813f;
        }

        private float SingleValueCubic(int seed, FNLfloat x, FNLfloat y)
        {
            int x1 = FastFloor(x);
            int y1 = FastFloor(y);

            float xs = (float)(x - x1);
            float ys = (float)(y - y1);

            x1 *= PrimeX;
            y1 *= PrimeY;
            int x0 = x1 - PrimeX;
            int y0 = y1 - PrimeY;
            int x2 = x1 + PrimeX;
            int y2 = y1 + PrimeY;
            int x3 = x1 + unchecked(PrimeX * 2);
            int y3 = y1 + unchecked(PrimeY * 2);

            return CubicLerp(
                CubicLerp(ValCoord(seed, x0, y0), ValCoord(seed, x1, y0), ValCoord(seed, x2, y0), ValCoord(seed, x3, y0),
                xs),
                CubicLerp(ValCoord(seed, x0, y1), ValCoord(seed, x1, y1), ValCoord(seed, x2, y1), ValCoord(seed, x3, y1),
                xs),
                CubicLerp(ValCoord(seed, x0, y2), ValCoord(seed, x1, y2), ValCoord(seed, x2, y2), ValCoord(seed, x3, y2),
                xs),
                CubicLerp(ValCoord(seed, x0, y3), ValCoord(seed, x1, y3), ValCoord(seed, x2, y3), ValCoord(seed, x3, y3),
                xs),
                ys) * (1 / (1.5f * 1.5f));
        }

        private float SingleValue(int seed, FNLfloat x, FNLfloat y)
        {
            int x0 = FastFloor(x);
            int y0 = FastFloor(y);

            float xs = InterpHermite((float)(x - x0));
            float ys = InterpHermite((float)(y - y0));

            x0 *= PrimeX;
            y0 *= PrimeY;
            int x1 = x0 + PrimeX;
            int y1 = y0 + PrimeY;

            float xf0 = Lerp(ValCoord(seed, x0, y0), ValCoord(seed, x1, y0), xs);
            float xf1 = Lerp(ValCoord(seed, x0, y1), ValCoord(seed, x1, y1), xs);

            return Lerp(xf0, xf1, ys);
        }

        private float GenNoiseSingle(int seed, FNLfloat x, FNLfloat y)
        {
            switch (mNoiseType)
            {
                case NoiseType.Perlin:
                    return SinglePerlin(seed, x, y);
                case NoiseType.ValueCubic:
                    return SingleValueCubic(seed, x, y);
                case NoiseType.Value:
                    return SingleValue(seed, x, y);
                default:
                    return 0;
            }
        }

        // Fractals

        private float GenFractalFBm(FNLfloat x, FNLfloat y)
        {
            int seed = mSeed;
            float sum = 0;
            float amp = mFractalBounding;

            for (int i = 0; i < mOctaves; i++)
            {
                float noise = GenNoiseSingle(seed++, x, y);
                sum += noise * amp;
                amp *= Lerp(1.0f, FastMin(noise + 1, 2) * 0.5f, mWeightedStrength);

                x *= mLacunarity;
                y *= mLacunarity;
                amp *= mGain;
            }

            return sum;
        }

        private float GenFractalRidged(FNLfloat x, FNLfloat y)
        {
            int seed = mSeed;
            float sum = 0;
            float amp = mFractalBounding;

            for (int i = 0; i < mOctaves; i++)
            {
                float noise = FastAbs(GenNoiseSingle(seed++, x, y));
                sum += (noise * -2 + 1) * amp;
                amp *= Lerp(1.0f, 1 - noise, mWeightedStrength);

                x *= mLacunarity;
                y *= mLacunarity;
                amp *= mGain;
            }

            return sum;
        }

        private float GenFractalPingPong(FNLfloat x, FNLfloat y)
        {
            int seed = mSeed;
            float sum = 0;
            float amp = mFractalBounding;

            for (int i = 0; i < mOctaves; i++)
            {
                float noise = PingPong((GenNoiseSingle(seed++, x, y) + 1) * mPingPongStrength);
                sum += (noise - 0.5f) * 2 * amp;
                amp *= Lerp(1.0f, noise, mWeightedStrength);

                x *= mLacunarity;
                y *= mLacunarity;
                amp *= mGain;
            }

            return sum;
        }

        // Domain Warp

        private void SingleDomainWarpBasicGrid(int seed, float warpAmp, float frequency, FNLfloat x, FNLfloat y, ref FNLfloat xr, ref FNLfloat yr)
        {
            FNLfloat xf = x * frequency;
            FNLfloat yf = y * frequency;

            int x0 = FastFloor(xf);
            int y0 = FastFloor(yf);

            float xs = InterpHermite((float)(xf - x0));
            float ys = InterpHermite((float)(yf - y0));

            x0 *= PrimeX;
            y0 *= PrimeY;
            int x1 = x0 + PrimeX;
            int y1 = y0 + PrimeY;

            int hash0 = Hash(seed, x0, y0) & (255 << 1);
            int hash1 = Hash(seed, x1, y0) & (255 << 1);

            float lx0x = Lerp(RandVecs2D[hash0], RandVecs2D[hash1], xs);
            float ly0x = Lerp(RandVecs2D[hash0 | 1], RandVecs2D[hash1 | 1], xs);

            hash0 = Hash(seed, x0, y1) & (255 << 1);
            hash1 = Hash(seed, x1, y1) & (255 << 1);

            float lx1x = Lerp(RandVecs2D[hash0], RandVecs2D[hash1], xs);
            float ly1x = Lerp(RandVecs2D[hash0 | 1], RandVecs2D[hash1 | 1], xs);

            xr += Lerp(lx0x, lx1x, ys) * warpAmp;
            yr += Lerp(ly0x, ly1x, ys) * warpAmp;
        }

        private void DoSingleDomainWarp(int seed, float amp, float freq, FNLfloat x, FNLfloat y, ref FNLfloat xr, ref FNLfloat yr)
        {
            switch (mDomainWarpType)
            {
                case DomainWarpType.BasicGrid:
                    SingleDomainWarpBasicGrid(seed, amp, freq, x, y, ref xr, ref yr);
                    break;
            }
        }

        private void DomainWarpSingle(ref FNLfloat x, ref FNLfloat y)
        {
            int seed = mSeed;
            float amp = mDomainWarpAmp * mFractalBounding;
            float freq = mFrequency;

            FNLfloat xs = x;
            FNLfloat ys = y;

            DoSingleDomainWarp(seed, amp, freq, xs, ys, ref x, ref y);
        }

        private void DomainWarpIndependent(ref FNLfloat x, ref FNLfloat y)
        {
            int seed = mSeed;
            float amp = mDomainWarpAmp * mFractalBounding;
            float freq = mFrequency;

            FNLfloat xs = x;
            FNLfloat ys = y;
            for (int i = 0; i < mOctaves; i++)
            {
                DoSingleDomainWarp(seed, amp, freq, xs, ys, ref x, ref y);

                seed++;
                amp *= mGain;
                freq *= mLacunarity;
            }
        }

        private void DomainWarpProgressive(ref FNLfloat x, ref FNLfloat y)
        {
            int seed = mSeed;
            float amp = mDomainWarpAmp * mFractalBounding;
            float freq = mFrequency;

            for (int i = 0; i < mOctaves; i++)
            {
                FNLfloat xs = x;
                FNLfloat ys = y;

                DoSingleDomainWarp(seed, amp, freq, xs, ys, ref x, ref y);

                seed++;
                amp *= mGain;
                freq *= mLacunarity;
            }
        }
    }
}
