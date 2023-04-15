using System;
using System.IO;

namespace MidiWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Generate a MIDI file with a melody
            GenerateMidiFile("A minor", new int[] { 4, 8, 16 }, 4, "output.mid");
        }

        static void GenerateMidiFile(string key, int[] noteLengths, int numBars, string filename)
        {
            // Create a MIDI file
            using (var writer = new BinaryWriter(File.Create(filename)))
            {
                // Write the MIDI header
                writer.Write(new byte[] { 0x4D, 0x54, 0x68, 0x64 });
                writer.Write(new byte[] { 0x00, 0x00, 0x00, 0x06 });
                writer.Write(new byte[] { 0x00, 0x01 });
                writer.Write(new byte[] { 0x00, 0x10 }); // 16 ticks per quarter note

                // Write the MIDI track header
                writer.Write(new byte[] { 0x4D, 0x54, 0x72, 0x6B });
                writer.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // placeholder for track length

                // Define variables for note and time values
                int ticksPerBeat = 16; // 16 ticks per quarter note
                int notesPerBeat = 4; // 4 sixteenth notes per quarter note
                int beatsPerMeasure = 4; // 4 beats per measure
                int measureLength = ticksPerBeat * beatsPerMeasure;

                // Define an array of note values for the melody
                int[] noteValues = GetNoteValuesInKey(key, numBars * beatsPerMeasure);

                // Define an array of note durations for the melody
                int[] noteDurations = GetNoteDurations(noteLengths, noteValues.Length);

                // Calculate the length of the MIDI track in ticks
                int trackLength = noteDurations.Length * measureLength;

                // Write the track length to the MIDI track header
                writer.Seek(18, SeekOrigin.Begin);
                writer.Write(GetBytes(trackLength, 4));

                // Write the note and timing data to the MIDI track
                int tickCounter = 0;
                int restDuration = 0;

                for (int i = 0; i < noteValues.Length; i++)
                {
                    // Write a rest note if necessary
                    if (restDuration > 0)
                    {
                        writer.Write(new byte[] { 0x00, 0x00, 0x00, (byte)restDuration });
                        tickCounter += restDuration;
                        restDuration = 0;
                    }

                    // Write the note
                    writer.Write(new byte[] { 0x00, 0x90, (byte)noteValues[i], 0x7F });

                    // Calculate the duration of the note in ticks
                    int duration = noteDurations[i] * measureLength / notesPerBeat;

                    // Write the note duration
                    writer.Write(new byte[] { 0x00, 0x00, 0x00, (byte)duration });
                    tickCounter += duration;
                    }}}}}
           

                    //
