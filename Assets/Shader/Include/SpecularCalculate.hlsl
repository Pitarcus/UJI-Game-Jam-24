void SpecularShading_float
  (
            float3 lightColor, // Sa
            float specularIntensity, // Sp
            float3 normal, // n
            float3 lightDir, // l
            float3 viewDir, // e
            float specularPower, // exponent
            out float color
            )
            {
                float3 h = normalize(lightDir + viewDir); // halfway
                color = lightColor.r * specularIntensity * pow(max(0, dot(normal, h)), specularPower);
            }