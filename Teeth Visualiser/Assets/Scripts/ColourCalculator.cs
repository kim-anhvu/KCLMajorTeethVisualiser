using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * This Class is responsible for finding out the colour gradient between two
 * radiation values a chosen number of colours.
 */

public static class ColourCalculator
{

    /*
     * Calculate the space needed to store the iterpolated colour 
     * values between pairs of colors.
     * @param colourPicked
     * @return int[]
     */
    private static int[] numberOfSegsEachColour(List<Color> colourPicked)
    {
        if (colourPicked.Count == 1)
        {
            colourPicked.Add(Color.grey);
            colourPicked.Reverse();
        }

        int[] colourSegments = new int[colourPicked.Count - 1];

        int numberOfPiecesForEachColour = 13 / (colourPicked.Count - 1);
        int max = 0;//6,12,18
        int difference = 0;
        if (numberOfPiecesForEachColour != 13)
        {
            while (max < 13)
            {
                max += numberOfPiecesForEachColour;

            }
            max -= numberOfPiecesForEachColour;
            difference = 13 - max;

            for (int i = 0; i < colourPicked.Count - 1; ++i)
            {
                if (difference != 0)
                {
                    colourSegments[i] = numberOfPiecesForEachColour + 1;
                    --difference;
                }
                else
                {
                    colourSegments[i] = numberOfPiecesForEachColour;
                }
            }
        }
        else
        {
            colourSegments[0] = numberOfPiecesForEachColour;
        }

        return colourSegments;
    }


    /*
     * Calculate the iterpolation values between colours.
     * @param radiationTo, radiationFrom, colourPicked
     * @return List<KeyValuePair<float, Color>>
     */
    public static List<KeyValuePair<float, Color>> CalculateColourScale(float radiationFrom, float radiationTo, List<Color> colourPicked)
    {
        if (radiationTo == -1 || radiationFrom == -1)
        {
            throw new Exception("No radiation values selected");
        }
        if (colourPicked.Count == 0)
        {
            throw new Exception("No colour's selected");
        }
        List<KeyValuePair<float, Color>> RadiationAndColour = new List<KeyValuePair<float, Color>>();
        int[] numberOfsegments = numberOfSegsEachColour(colourPicked);
        float RadMap = radiationFrom;

        float R1;
        float G1;
        float B1;

        float R;
        float G;
        float B;
        int k = 0;
        int m = 0;
        int l = numberOfsegments[0]; ;

        for (int j = 0; j < colourPicked.Count - 1; ++j)
        {

            R = colourPicked[j].r;
            G = colourPicked[j].g;
            B = colourPicked[j].b;

            R1 = colourPicked[j + 1].r;
            G1 = colourPicked[j + 1].g;
            B1 = colourPicked[j + 1].b;

            float RToAdd = R1 - R;
            float GToAdd = G1 - G;
            float BToAdd = B1 - B;

            float radiation = (radiationTo - radiationFrom) / 12;
            RToAdd = RToAdd / (numberOfsegments[j] - 1);
            GToAdd = GToAdd / (numberOfsegments[j] - 1);
            BToAdd = BToAdd / (numberOfsegments[j] - 1);

            Color x = new Color(R, G, B);
            //change  

            for (k = m; k < l; k++)
            {
                RadiationAndColour.Add(new KeyValuePair<float, Color>(RadMap, x));
                x = new Color(x.r + RToAdd, x.g + GToAdd, x.b + BToAdd);
                RadMap = RadMap + radiation;
                m++;
            }

            if (j + 1 < numberOfsegments.Length)
            {
                l += numberOfsegments[j + 1];
            }
        }
        return RadiationAndColour;
    }

}

