<?xml version="1.0" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">
  <xsl:output method='xml' version='1.0'/>
  <xsl:template match="DTS">
    <ss:Workbook >
      <ss:Styles>
        <ss:Style ss:ID="Default">
          <ss:NumberFormat ss:Format="General"/>
        </ss:Style>
        <ss:Style ss:ID="DateTime">
          <ss:NumberFormat ss:Format="General Date"/>
        </ss:Style>
      </ss:Styles>
      <xsl:apply-templates select="DT" />
    </ss:Workbook>
  </xsl:template>

  <xsl:template match="DT">
    <ss:Worksheet>
      <xsl:attribute  name="ss:Name">
        <xsl:value-of select="@N"/>
      </xsl:attribute>
      <ss:Table>
        <xsl:apply-templates select="DR" />
      </ss:Table>
    </ss:Worksheet>
  </xsl:template>

  <xsl:template match="DR">
    <ss:Row>
      <xsl:apply-templates select="DC" />
    </ss:Row>
  </xsl:template>

  <xsl:template match="DC">
    <ss:Cell>
      <xsl:choose>
        <xsl:when test="@T = 'DateTime'">
          <xsl:attribute name="ss:StyleID">
            <xsl:text>DateTime</xsl:text>
          </xsl:attribute>
        </xsl:when>
      </xsl:choose>
      <ss:Data>
        <xsl:attribute name="ss:Type">
          <xsl:value-of select="@T"/>
        </xsl:attribute>
        <xsl:choose>
          <xsl:when test="@T = 'String'">
            <xsl:text disable-output-escaping="yes">&lt;![CDATA[</xsl:text>
            <xsl:value-of  select="."/>
            <xsl:text disable-output-escaping="yes">]]&gt;</xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="."/>
          </xsl:otherwise>
        </xsl:choose>
      </ss:Data>
    </ss:Cell>
  </xsl:template>
</xsl:stylesheet>