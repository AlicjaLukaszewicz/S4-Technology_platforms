package org.example;

import org.apache.commons.lang3.tuple.Pair;

import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.concurrent.ForkJoinPool;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class Main {
    public static void main(String[] args) {
        // Check if the source directory path was provided as a command-line argument
        if (args.length == 0) {
            System.out.println("Please provide the source directory path as a command-line argument.");
            return;
        }

        // Source directory path
        Path source = Path.of(args[0]);

        // Collect all files in the source directory
        List<Path> files;
        try (Stream<Path> stream = Files.list(source)) {
            // Collect all files in the source directory into a list of paths
            files = stream.collect(Collectors.toList());
        } catch (IOException e) {
            e.printStackTrace();
            return;
        }

        for (int i = 1; i <= 4; i++) {
            // Create a ForkJoinPool with the specified number of threads
            ForkJoinPool pool = new ForkJoinPool(i);
            long start = System.currentTimeMillis();
            try {
                // Submit a task to the pool
                pool.submit(() -> files.parallelStream()
                        // Process each file in parallel using the pool of threads in the ForkJoinPool instance and return a stream of pairs of file names and images
                        .map(path -> {
                            try {
                                // Read the image from the file and return a pair of the file name and the image
                                return Pair.of(path.getFileName().toString(), ImageIO.read(path.toFile()));
                            } catch (IOException e) {
                                e.printStackTrace();
                                return null;
                            }
                        }).map(pair -> {
                            // Create a new BufferedImage with the same dimensions and type as the original
                            BufferedImage original = pair.getValue();
                            BufferedImage image = new BufferedImage(original.getWidth(), original.getHeight(), original.getType());

                            // Copy pixels from the original image to the new image without any modification
                            for (int x = 0; x < original.getWidth(); x++) {
                                for (int y = 0; y < original.getHeight(); y++) {
                                    int rgb = original.getRGB(x, y);
                                    image.setRGB(x, y, rgb);
                                }
                            }

                            // Return a pair of the file name and the new image (which is the same as the original)
                            return Pair.of(pair.getKey(), image);
                        }).map(pair -> {
                            // Create another new BufferedImage with the same dimensions and type
                            BufferedImage original = pair.getValue();

                            // Create a new BufferedImage with the same dimensions and type as the original
                            BufferedImage image = new BufferedImage(original.getWidth(), original.getHeight(), original.getType());

                            // Modify each pixel in the image by changing its color components
                            for (int x = 0; x < original.getWidth(); x++) {
                                for (int y = 0; y < original.getHeight(); y++) {
                                    int rgb = original.getRGB(x, y);
                                    Color color = new Color(rgb);

                                    // Extract the red, blue, and green components of the color
                                    int red = color.getRed();
                                    int blue = color.getBlue();
                                    int green = color.getGreen();

                                    // Create a new color with modified components
                                    Color outColor = new Color(red, blue, green);
                                    int outRgb = outColor.getRGB();

                                    // Set the modified pixel in the new image
                                    image.setRGB(x, y, outRgb);
                                }
                            }

                            // Return a pair of the file name and the new modified image
                            return Pair.of(pair.getKey(), image);
                        }).forEach(pair -> {
                            try {
                                // Write the modified image to a new file in the destination directory
                                ImageIO.write(pair.getValue(), "jpg", Path.of("C:\\Users\\Alicja\\Desktop\\images2\\" + pair.getKey()).toFile());
                            } catch (IOException e) {
                                e.printStackTrace();
                            }
                        })).get();
            } catch (Exception e) {
                e.printStackTrace();
            }
            // Print the time of processing for the specified number of threads
            System.out.println("For pool size " + i + ": time of processing = " + (System.currentTimeMillis() - start) + "ms");

            // Shut down the pool of threads in the ForkJoinPool instance
            pool.shutdown();
        }
    }
}
