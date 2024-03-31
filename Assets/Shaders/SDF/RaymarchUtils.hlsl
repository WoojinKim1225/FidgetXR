#ifndef RAYMARCHUTILS_INCLUDED
#define RAYMARCHUTILS_INCLUDED

float smoothMax(float a, float b, float k) {
    return log(exp(k * a) + exp(k * b)) / k;
}

float smoothMin(float a, float b, float k) {
    return -smoothMax(-a, -b, k);
}

float sdCylinder (float3 p, float3 a, float3 b, float ro, float ri) {
    float3 ab = b - a;
    float3 ap = p - a;
    float t = dot(ap, ab) / dot (ab, ab);
    float3 c = a + t * ab;
    float d = length(p - c) - ro + ri;

    float y =(abs(t - 0.5) - 0.5) * length(ab);
    float e = length(max(float2(d, y), 0));
    float i = min(max(d,y), 0);
    return e + i - ri;
}

float sdRoundCone(float3 p, float3 a, float3 b, float r1, float r2)
{
  // sampling independent computations (only depend on shape)
  float3  ba = b - a;
  float l2 = dot(ba,ba);
  float rr = r1 - r2;
  float a2 = l2 - rr*rr;
  float il2 = 1.0/l2;
    
  // sampling dependant computations
  float3 pa = p - a;
  float y = dot(pa,ba);
  float z = y - l2;
  float x2 = dot(pa*l2 - ba*y , pa*l2 - ba*y);
  float y2 = y*y*l2;
  float z2 = z*z*l2;

  // single square root!
  float k = sign(rr)*rr*rr*x2;
  if( sign(z)*a2*z2>k ) return  sqrt(x2 + z2)        *il2 - r2;
  if( sign(y)*a2*y2<k ) return  sqrt(x2 + y2)        *il2 - r1;
                        return (sqrt(x2*a2*il2)+y*rr)*il2 - r1;
}

float sdCapsule(float3 p, float3 a, float3 b, float r) {
    float3 ab = b - a;
    float3 ap = p - a;
    float t = dot(ap, ab) / dot (ab, ab);
    t = clamp(t, 0, 1);
    float3 c = a + t * ab;
    return length(p - c) - r;
}

float sdBox(float3 p, float3 c, float3 size, float3 r) {
    return length(max(abs(p - c) - size + r, 0)) - r;
}

float sdSphere(float3 p, float3 c, float r) {
    return length(p - c) - r;
}

#endif