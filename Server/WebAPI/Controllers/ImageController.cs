﻿using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile image, [FromForm] Guid gameId)
        {
            var createdImage = await _imageService.UploadImageAsync(GetImageModel(image, gameId));
            return CreatedAtAction(nameof(UploadImage), new { id = createdImage.Id }, createdImage);
        }

        [HttpGet]
        public async Task<IActionResult> GetImages([FromQuery] string id)
        {
            var gameImages = await _imageService.GetImagesByGameIdAsync(Guid.Parse(id));
            return Ok(gameImages);
        }

        public async Task<IActionResult> DeleteImage([FromBody] ImageModel model)
        {
            await _imageService.DeleteAsync(model);
            return Ok("Deleted successfully");
        }

        private static ImageModel GetImageModel(IFormFile image, Guid gameId)
        {
            byte[]? imageData = null;
            using (var binaryReader = new BinaryReader(image.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)image.Length);
            }
            return new ImageModel
            {
                GameId = gameId,
                Image = imageData
            };
        }
    }
}
